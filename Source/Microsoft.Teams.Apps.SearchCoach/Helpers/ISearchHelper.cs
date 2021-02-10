﻿// <copyright file="ISearchHelper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Helpers
{
    using Microsoft.Teams.Apps.SearchCoach.Models.BingSearch;

    /// <summary>
    /// Interface for Bing search methods.
    /// </summary>
    public interface ISearchHelper
    {
        /// <summary>
        /// Validate selected country code value is valid or not.
        /// </summary>
        /// <param name="selectedCountryCode">Selected country code value.</param>
        /// <returns>Returns whether selected country code is valid or not.</returns>
        bool IsValidCountry(string selectedCountryCode);

        /// <summary>
        /// Validate selected domain value is valid or not.
        /// </summary>
        /// <param name="selectedDomainValue">Selected domain value.</param>
        /// <returns>Returns whether selected domain value is valid or not.</returns>
        bool IsValidDomain(string selectedDomainValue);

        /// <summary>
        /// Construct search query model data.
        /// </summary>
        /// <param name="searchFilterModel">Selected search filters.</param>
        /// <returns>Returns search query model.</returns>
        SearchQuery ConstructSearchQueryModel(
            SearchFilterModel searchFilterModel);
    }
}