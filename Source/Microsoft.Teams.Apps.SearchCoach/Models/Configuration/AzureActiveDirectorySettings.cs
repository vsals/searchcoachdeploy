// <copyright file="AzureActiveDirectorySettings.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.Configuration
{
    /// <summary>
    /// A class which helps to provide Azure Active Directory settings for application.
    /// </summary>
    public class AzureActiveDirectorySettings
    {
        /// <summary>
        /// Gets or sets Graph API scope.
        /// </summary>
        public string GraphScope { get; set; }
    }
}