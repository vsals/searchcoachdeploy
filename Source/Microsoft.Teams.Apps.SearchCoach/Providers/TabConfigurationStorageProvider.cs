// <copyright file="TabConfigurationStorageProvider.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Providers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// A class that contains tab configuration storage provider methods.
    /// </summary>
    public class TabConfigurationStorageProvider : BaseStorageProvider, ITabConfigurationStorageProvider
    {
        private const string TeamTabConfigurationTable = "TabConfigurationEntity";

        /// <summary>
        /// Initializes a new instance of the <see cref="TabConfigurationStorageProvider"/> class.
        /// </summary>
        /// <param name="options">A set of key/value application configuration properties for Microsoft Azure Table storage.</param>
        /// <param name="logger">Sends logs to the logger service.</param>
        public TabConfigurationStorageProvider(
            IOptions<StorageSettings> options,
            ILogger<TabConfigurationStorageProvider> logger)
            : base(options?.Value.ConnectionString, TeamTabConfigurationTable, logger)
        {
        }

        /// <summary>
        /// Store or update team's tab configuration details in Azure storage.
        /// </summary>
        /// <param name="teamEntity">Tab configuration entity used for storage and retrieval.</param>
        /// <returns><see cref="Task"/>Returns the status whether tab configuration entity is stored or not.</returns>
        public async Task<bool> UpsertTabConfigurationDetailAsync(TabConfiguration teamEntity)
        {
            var result = await this.StoreOrUpdateEntityAsync(teamEntity);

            return result.HttpStatusCode == (int)HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Get team's tab configuration entity details.
        /// </summary>
        /// <param name="teamId">Team id of the configured tab.</param>
        /// <param name="tabId">Tab id of the configured tab in a team.</param>
        /// <returns>A tab configuration entity details.</returns>
        public async Task<TabConfiguration> GetTabConfigurationEntityAsync(string teamId, string tabId)
        {
            await this.EnsureInitializedAsync();

            var operation = TableOperation.Retrieve<TabConfiguration>(teamId, tabId);
            var data = await this.CloudTable.ExecuteAsync(operation);

            return data.Result as TabConfiguration;
        }

        /// <summary>
        /// Stores or update tab configuration details in Microsoft Azure Table storage.
        /// </summary>
        /// <param name="entity">Holds tab configuration detail entity data.</param>
        /// <returns>A task that represents tab configuration entity data is saved or updated.</returns>
        private async Task<TableResult> StoreOrUpdateEntityAsync(TabConfiguration entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(entity.TeamId) || string.IsNullOrWhiteSpace(entity.TabId))
            {
                return null;
            }

            await this.EnsureInitializedAsync();
            TableOperation addOrUpdateOperation = TableOperation.InsertOrReplace(entity);
            return await this.CloudTable.ExecuteAsync(addOrUpdateOperation);
        }
    }
}