// <copyright file="TabConfiguration.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// A class which represents tab configuration entity model, this class is used to store the mapping of teams tab with it's group id.
    /// </summary>
    public partial class TabConfiguration : TableEntity
    {
        /// <summary>
        /// Gets or sets team id where tab is configured.
        /// </summary>
        [Required]
        public string TeamId
        {
            get { return this.PartitionKey; }
            set { this.PartitionKey = value; }
        }

        /// <summary>
        /// Gets or sets tab id of configured tab.
        /// </summary>
        [Required]
        public string TabId
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }

        /// <summary>
        /// Gets or sets group id of team where tab is configured.
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets Azure Active Directory id of user who configured the tab.
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets tab configuration created on date.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }
    }
}