// <copyright file="SearchQuery.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.BingSearch
{
    /// <summary>
    /// Bing search query model.
    /// </summary>
    public class SearchQuery : SearchFilterModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether error is there or not.
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets offset.
        /// Offset determines the number of records to be skipped before returning results.
        /// Default value is 0.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets application id.
        /// </summary>
        public string AppId { get; set; }
    }
}