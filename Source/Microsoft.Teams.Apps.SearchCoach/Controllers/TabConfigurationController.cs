// <copyright file="TabConfigurationController.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Authentication;
    using Microsoft.Teams.Apps.SearchCoach.Common;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;
    using Microsoft.Teams.Apps.SearchCoach.Providers;

    /// <summary>
    /// Controller to handle tab configuration API operations.
    /// </summary>
    [Route("api/tabconfiguration")]
    [ApiController]
    [Authorize]
    public class TabConfigurationController : BaseSearchCoachController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger<TabConfigurationController> logger;

        /// <summary>
        /// Tab configuration provider to deal with configuration storage methods.
        /// </summary>
        private readonly ITabConfigurationStorageProvider tabConfigurationStorageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabConfigurationController"/> class.
        /// </summary>
        /// <param name="logger">Logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="tabConfigurationStorageProvider">Tab configuration provider to deal with configuration storage methods.</param>
        public TabConfigurationController(
            ILogger<TabConfigurationController> logger,
            TelemetryClient telemetryClient,
            ITabConfigurationStorageProvider tabConfigurationStorageProvider)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.tabConfigurationStorageProvider = tabConfigurationStorageProvider;
        }

        /// <summary>
        /// Post call to store team's tab configuration details in storage.
        /// </summary>
        /// <param name="teamId">Team id where tab is configured.</param>
        /// <param name="groupId">Group id of the current team.</param>
        /// <returns>Returns true for successful operation.</returns>
        [HttpPost("{teamId}")]
        [Authorize(PolicyNames.MustBeTeamMemberUserPolicy)]
        public async Task<IActionResult> PostAsync(
            string teamId,
            [FromQuery] string groupId)
        {
            this.RecordEvent("TabConfiguration - HTTP Post call.", RequestStatus.Initiated);
            this.logger.LogInformation("Call to add tab configuration details.");

            try
            {
                var tabConfigurationEntityModel = new TabConfiguration()
                {
                    // Intentionally fetching group id from query string because it is validated by MustBeTeamMemberUserPolicy.
                    TeamId = teamId,
                    TabId = Guid.NewGuid().ToString(),
                    GroupId = Guid.Parse(groupId),
                    CreatedByUserId = Guid.Parse(this.UserAadId),
                    CreatedOn = DateTime.UtcNow,
                };

                // Store tab configuration details in storage.
                var result = await this.tabConfigurationStorageProvider.UpsertTabConfigurationDetailAsync(tabConfigurationEntityModel);

                if (result)
                {
                    this.RecordEvent("TabConfiguration - HTTP Post call.", RequestStatus.Succeeded);
                    return this.Ok(tabConfigurationEntityModel);
                }

                this.RecordEvent("TabConfiguration - HTTP Post call.", RequestStatus.Failed);
                return this.BadRequest("Error while saving tab configuration details to storage.");
            }
            catch (Exception ex)
            {
                this.RecordEvent("TabConfiguration - HTTP Post call.", RequestStatus.Failed);
                this.logger.LogError(ex, $"Error while saving tab configuration details.");
                throw;
            }
        }
    }
}