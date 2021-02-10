// <copyright file="ITabConfigurationStorageProvider.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Providers
{
    using System.Threading.Tasks;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;

    /// <summary>
    /// Interface for team's tab configuration storage provider.
    /// </summary>
    public interface ITabConfigurationStorageProvider
    {
        /// <summary>
        /// Store or update team's tab configuration details in Azure storage.
        /// </summary>
        /// <param name="teamEntity">Tab configuration entity used for storage and retrieval.</param>
        /// <returns><see cref="Task"/>Returns the status whether tab configuration entity is stored or not.</returns>
        Task<bool> UpsertTabConfigurationDetailAsync(TabConfiguration teamEntity);

        /// <summary>
        /// Get team's tab configuration entity details.
        /// </summary>
        /// <param name="teamId">Team id of the configured tab.</param>
        /// <param name="tabId">Tab id of the configured tab in a team.</param>
        /// <returns>A tab configuration entity details.</returns>
        Task<TabConfiguration> GetTabConfigurationEntityAsync(string teamId, string tabId);
    }
}