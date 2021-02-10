// <copyright file="TabConfigurationData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using System;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;

    /// <summary>
    /// Class that contains test data for tab configuration.
    /// </summary>
    public static class TabConfigurationData
    {
        /// <summary>
        /// Test data for tab configuration entity.
        /// </summary>
        public static readonly TabConfiguration TabConfigurationEntity = new TabConfiguration()
        {
            TabId = "00000000-0000-0000-0000-000000000009",
            GroupId = Guid.Parse("00000000-0000-0000-0000-000000000009"),
            TeamId = "00:0000000000000000000000000000000@abc.tacv0",
            CreatedByUserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        };

        /// <summary>
        /// Fetch team member's details based on group id of team.
        /// </summary>
        public static readonly string GroupId = "00000000-0000-0000-0000-000000000009";

        /// <summary>
        /// Test data for team id.
        /// </summary>
        public static readonly string TeamId = "00:0000000000000000000000000000000@abc.tacv0";
    }
}