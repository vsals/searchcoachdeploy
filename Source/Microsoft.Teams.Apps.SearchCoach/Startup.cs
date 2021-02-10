﻿// <copyright file="Startup.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach
{
    using System;
    using System.Net.Http;
    using global::Azure.Identity;
    using global::Azure.Security.KeyVault.Secrets;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Teams.Apps.SearchCoach.Authentication;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Polly;
    using Polly.Extensions.Http;

    /// <summary>
    /// The Startup class is responsible for configuring the DI container and acts as the composition root.
    /// </summary>
    public sealed class Startup
    {
        /// <summary>
        /// Gets the IConfiguration instance.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">IConfiguration instance.</param>
#pragma warning disable SA1201 // Declare property before initializing in constructor.
        public Startup(IConfiguration configuration)
#pragma warning restore SA1201 // Declare property before initializing in constructor.
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var useKeyVault = this.configuration.GetValue<bool>("UseKeyVault");

            if (useKeyVault)
            {
                this.GetKeyVaultByManagedServiceIdentity();
            }
        }

        /// <summary>
        /// Configure the composition root for the application.
        /// </summary>
        /// <param name="services">The stub composition root.</param>
        /// <remarks>
        /// For more information see: https://go.microsoft.com/fwlink/?LinkID=398940.
        /// </remarks>
#pragma warning disable CA1506 // Composition root expected to have coupling with many components.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddConfidentialCredentialProvider(this.configuration);
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddSearchCoachAuthentication(this.configuration);
            services.AddSingleton<IChannelProvider, SimpleChannelProvider>();
            services.AddHttpClient<ISearchHelper, BingSearchHelper>().AddPolicyHandler(GetRetryPolicy());
            services.AddHttpClient<IBingSearchProvider, BingSearchHelper>().AddPolicyHandler(GetRetryPolicy());
            services.AddConfigurationSettings(this.configuration);
            services.AddCredentialProviders(this.configuration);
            services.AddProviders(this.configuration);
            services.AddHelpers(this.configuration);
            services.AddBotStates(this.configuration);
            services.AddServices();

            // In production, the React files will be served from this directory.
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMemoryCache();
            services.AddBotFrameworkAdapter();

            // Add i18n.
            services.AddLocalizationSettings(this.configuration);
        }
#pragma warning restore CA1506

        /// <summary>
        /// Configure the application request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">Hosting Environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseMvc();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.EnvironmentName.ToUpperInvariant() == "DEVELOPMENT")
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        /// <summary>
        /// Retry policy for transient error cases.
        /// If there is no success code in response, request will be sent again for two times
        /// with interval of 2 and 4 seconds respectively.
        /// </summary>
        /// <returns>Policy.</returns>
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => response.IsSuccessStatusCode == false)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        /// Get KeyVault secrets and set app-settings values.
        /// </summary>
        private void GetKeyVaultByManagedServiceIdentity()
        {
            // Create a new secret client using the default credential from Azure.Identity using environment variables.
            var client = new SecretClient(
                vaultUri: new Uri($"{this.configuration["KeyVaultUrl:BaseURL"]}/"),
                credential: new DefaultAzureCredential());

            this.configuration["MicrosoftAppId"] = client.GetSecret("MicrosoftAppId").Value.Value;
            this.configuration["MicrosoftAppPassword"] = client.GetSecret("MicrosoftAppPassword").Value.Value;
            this.configuration["Storage:ConnectionString"] = client.GetSecret("StorageConnection--SecretKey").Value.Value;
            this.configuration["BingSearch:ApiKey"] = client.GetSecret("BingSearchApiKey").Value.Value;
        }
    }
}