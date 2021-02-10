﻿// <copyright file="SearchFilterModel.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.BingSearch
{
    using System.Collections.Generic;

    /// <summary>
    /// Bing search filter model.
    /// </summary>
    public class SearchFilterModel
    {
        /// <summary>
        /// Gets or sets search text for Bing API.
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Gets or sets search domains for Bing API.
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        public List<string> Domains { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        /// <summary>
        /// Gets or sets time freshness of contents.
        /// </summary>
        public string Freshness { get; set; }

        /// <summary>
        /// Gets or sets market value.
        /// Market value indicates the country-code selected through location filter.
        /// </summary>
        public string Market { get; set; }
    }
}