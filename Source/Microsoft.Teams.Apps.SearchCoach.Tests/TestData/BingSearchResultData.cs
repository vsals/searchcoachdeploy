// <copyright file="BingSearchResultData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using System.Collections.Generic;
    using Microsoft.Teams.Apps.SearchCoach.Models.BingSearch;

    /// <summary>
    /// Class that contains test data for bing search results.
    /// </summary>
    public static class BingSearchResultData
    {
        /// <summary>
        /// Search string value entered by user.
        /// </summary>
        public static readonly string SearchString = "COVID";

        /// <summary>
        /// Search string value with HTML content entered by user.
        /// </summary>
        public static readonly string SearchStringWithHtmlContent = "COVID <html></html>";

        /// <summary>
        /// Search string value with JavaScript content entered by user.
        /// </summary>
        public static readonly string SearchStringWithScriptContent = "COVID <script></script>";

        /// <summary>
        /// Market value selected by user using filters.
        /// </summary>
        public static readonly string MarketValue = "en-US";

        /// <summary>
        /// Domain value selected by user using filters.
        /// </summary>
        public static readonly List<string> DomainValues = new List<string> { ".com" };

        /// <summary>
        /// Invalid Domain value as an input scenario.
        /// </summary>
        public static readonly List<string> InvalidDomainValues = new List<string> { ".us" };

        /// <summary>
        /// Freshness value selected by user using filters.
        /// </summary>
        public static readonly string FreshnessValue = "Day";

        /// <summary>
        /// Safe search value to be passed to Bing API.
        /// </summary>
        public static readonly string SafeSearch = "safe";

        /// <summary>
        /// The AppId value to be passed to Bing API.
        /// </summary>
        public static readonly string AppId = "12345";

        /// <summary>
        /// A search query object containing parameters to be passed to Bing API.
        /// </summary>
        public static readonly SearchQuery SearchQuery = new SearchQuery
        {
            SearchText = SearchString,
            Domains = DomainValues,
            Error = false,
            Count = 20,
            Offset = 0,
            Freshness = FreshnessValue,
            AppId = AppId,
            Market = MarketValue,
        };

        /// <summary>
        /// A search query object having HTML content in searched text to be passed to Bing API.
        /// </summary>
        public static readonly SearchQuery SearchQueryWithSearchTextAsHtml = new SearchQuery
        {
            SearchText = SearchStringWithHtmlContent,
            Domains = InvalidDomainValues,
            Error = false,
            Count = 20,
            Offset = 0,
            Freshness = FreshnessValue,
            AppId = AppId,
            Market = MarketValue,
        };

        /// <summary>
        /// A search query object having JavaScript content in searched text to be passed to Bing API.
        /// </summary>
        public static readonly SearchQuery SearchQueryWithSearchTextAsScript = new SearchQuery
        {
            SearchText = SearchStringWithScriptContent,
            Domains = DomainValues,
            Error = false,
            Count = 20,
            Offset = 0,
            Freshness = FreshnessValue,
            AppId = AppId,
            Market = MarketValue,
        };

        /// <summary>
        /// A search query object having invalid domain value to be passed to Bing API.
        /// </summary>
        public static readonly SearchQuery SearchQueryWithInvalidDomain = new SearchQuery
        {
            SearchText = SearchString,
            Domains = InvalidDomainValues,
            Error = false,
            Count = 20,
            Offset = 0,
            Freshness = FreshnessValue,
            AppId = AppId,
            Market = MarketValue,
        };
    }
}