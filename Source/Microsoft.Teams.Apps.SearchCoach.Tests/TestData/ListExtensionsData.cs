// <copyright file="ListExtensionsData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using System.Collections.Generic;

    /// <summary>
    /// Class that contains test data for list extension methods.
    /// </summary>
    public static class ListExtensionsData
    {
        /// <summary>
        /// A collection of user object id's.
        /// </summary>
        public static readonly List<string> UserObjectIds = new List<string>()
        {
            "00000000-0000-0000-0000-000000000001",
            "00000000-0000-0000-0000-000000000002",
            "00000000-0000-0000-0000-000000000003",
            "00000000-0000-0000-0000-000000000004",
            "00000000-0000-0000-0000-000000000005",
            "00000000-0000-0000-0000-000000000006",
            "00000000-0000-0000-0000-000000000007",
            "00000000-0000-0000-0000-000000000008",
            "00000000-0000-0000-0000-000000000009",
            "00000000-0000-0000-0000-000000000010",
            "00000000-0000-0000-0000-000000000011",
            "00000000-0000-0000-0000-000000000012",
            "00000000-0000-0000-0000-000000000013",
            "00000000-0000-0000-0000-000000000014",
            "00000000-0000-0000-0000-000000000015",
            "00000000-0000-0000-0000-000000000016",
            "00000000-0000-0000-0000-000000000017",
            "00000000-0000-0000-0000-000000000018",
            "00000000-0000-0000-0000-000000000019",
            "00000000-0000-0000-0000-000000000020",
            "00000000-0000-0000-0000-000000000021",
            "00000000-0000-0000-0000-000000000022",
        };

        /// <summary>
        /// A empty list for user object id's.
        /// </summary>
        public static readonly List<string> UserObjectIdsEmptyList = new List<string>();

        /// <summary>
        /// Batch size for splitting the list of items.
        /// </summary>
        public static readonly int BatchSplitCount = 20;
    }
}