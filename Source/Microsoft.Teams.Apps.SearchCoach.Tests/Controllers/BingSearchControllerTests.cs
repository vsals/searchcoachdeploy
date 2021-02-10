// <copyright file="BingSearchControllerTests.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Controllers;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.Models.BingSearch;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Microsoft.Teams.Apps.SearchCoach.Tests.Fakes;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Class that contains test cases related to Bing Search API controller.
    /// </summary>
    [TestClass]
    public class BingSearchControllerTests
    {
        private Mock<ILogger<BingSearchController>> logger;
        private TelemetryClient telemetryClient;
        private BingSearchController bingSearchController;
        private Mock<ISearchHelper> bingSearchHelper;
        private Mock<IBingSearchProvider> bingSearchProvider;
        private SearchFilterModel searchFilterModel;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logger = new Mock<ILogger<BingSearchController>>();
#pragma warning disable CS0618 // Type or member is obsolete
            this.telemetryClient = new TelemetryClient();
#pragma warning restore CS0618 // Type or member is obsolete
            this.bingSearchHelper = new Mock<ISearchHelper>();
            this.bingSearchProvider = new Mock<IBingSearchProvider>();

            this.bingSearchController = new BingSearchController(
                this.logger.Object,
                this.telemetryClient,
                this.bingSearchHelper.Object,
                this.bingSearchProvider.Object);

            this.bingSearchController.ControllerContext = new ControllerContext();
            this.bingSearchController.ControllerContext.HttpContext = FakeHttpContext.GetMockHttpContextWithUserClaims();
        }

        /// <summary>
        /// Test case to check if valid input parameters are passed and returns search results.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task BingSearchValidInputValidContentsAsync()
        {
            // ARRANGE
            this.bingSearchProvider
                .Setup(x => x.GetBingSearchResultsAsync(BingSearchResultData.SearchQuery))
                .Returns(Task.FromResult(new List<BingWebPagesResult>().AsEnumerable()));

            this.bingSearchHelper
                .Setup(x => x.IsValidCountry(BingSearchResultData.MarketValue))
                .Returns(true);

            this.bingSearchHelper
                .Setup(x => x.IsValidDomain(BingSearchResultData.DomainValues[0]))
                .Returns(true);

            this.searchFilterModel = new SearchFilterModel
            {
                Domains = BingSearchResultData.DomainValues,
                Freshness = BingSearchResultData.FreshnessValue,
                Market = BingSearchResultData.MarketValue,
                SearchText = BingSearchResultData.SearchString,
            };

            // ACT
            var result = (ObjectResult)await this.bingSearchController.GetSearchResultAsync(
                this.searchFilterModel).ConfigureAwait(false);

            // ASSERT
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Test case to check if invalid domain values are passed in method and it returns bad request.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task BingSearchInvalidDomainBadRequestAsync()
        {
            // ARRANGE
            this.bingSearchProvider.Setup(x => x.GetBingSearchResultsAsync(null))
                .Returns(Task.FromResult(new List<BingWebPagesResult>().AsEnumerable()));

            this.bingSearchHelper
                .Setup(x => x.IsValidCountry(BingSearchResultData.MarketValue))
                .Returns(true);

            this.bingSearchHelper
                .Setup(x => x.IsValidDomain(BingSearchResultData.InvalidDomainValues[0]))
                .Returns(false);

            this.searchFilterModel = new SearchFilterModel
            {
                Domains = BingSearchResultData.InvalidDomainValues,
                Freshness = BingSearchResultData.FreshnessValue,
                Market = BingSearchResultData.MarketValue,
                SearchText = BingSearchResultData.SearchString,
            };

            // ACT
            var result = (ObjectResult)await this.bingSearchController.GetSearchResultAsync(
                this.searchFilterModel).ConfigureAwait(false);

            // ASSERT
            Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Test case to check if search text contains HTML tags and returns valid results or not.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task BingSearchTextWithHtmlValidContentsAsync()
        {
            // ARRANGE
            this.bingSearchProvider
                .Setup(x => x.GetBingSearchResultsAsync(BingSearchResultData.SearchQueryWithSearchTextAsHtml))
                .Returns(Task.FromResult(new List<BingWebPagesResult>().AsEnumerable()));

            this.bingSearchHelper
                .Setup(x => x.IsValidCountry(BingSearchResultData.MarketValue))
                .Returns(true);

            this.bingSearchHelper
                .Setup(x => x.IsValidDomain(BingSearchResultData.DomainValues[0]))
                .Returns(true);

            this.searchFilterModel = new SearchFilterModel
            {
                Domains = BingSearchResultData.DomainValues,
                Freshness = BingSearchResultData.FreshnessValue,
                Market = BingSearchResultData.MarketValue,
                SearchText = BingSearchResultData.SearchStringWithHtmlContent,
            };

            // ACT
            var result = (ObjectResult)await this.bingSearchController.GetSearchResultAsync(
                this.searchFilterModel).ConfigureAwait(false);

            // ASSERT
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Test case to check if search text contains scripts and returns valid results or not.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task BingSearchTextWithScriptsValidContentsAsync()
        {
            // ARRANGE
            this.bingSearchProvider
                .Setup(x => x.GetBingSearchResultsAsync(BingSearchResultData.SearchQueryWithSearchTextAsScript))
                .Returns(Task.FromResult(new List<BingWebPagesResult>().AsEnumerable()));

            this.bingSearchHelper
                .Setup(x => x.IsValidCountry(BingSearchResultData.MarketValue))
                .Returns(true);

            this.bingSearchHelper
                .Setup(x => x.IsValidDomain(BingSearchResultData.DomainValues[0]))
                .Returns(true);

            this.searchFilterModel = new SearchFilterModel
            {
                Domains = BingSearchResultData.DomainValues,
                Freshness = BingSearchResultData.FreshnessValue,
                Market = BingSearchResultData.MarketValue,
                SearchText = BingSearchResultData.SearchStringWithScriptContent,
            };

            // ACT
            var result = (ObjectResult)await this.bingSearchController.GetSearchResultAsync(
                this.searchFilterModel).ConfigureAwait(false);

            // ASSERT
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
        }
    }
}