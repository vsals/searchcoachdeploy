// <copyright file="TabConfigurationControllerTest.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Controllers;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Microsoft.Teams.Apps.SearchCoach.Tests.Fakes;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Test class that contains test cases related to tab configuration controller methods.
    /// </summary>
    [TestClass]
    public class TabConfigurationControllerTest
    {
        private Mock<ILogger<TabConfigurationController>> logger;
        private TelemetryClient telemetryClient;
        private TabConfigurationController tabConfigurationController;
        private Mock<ITabConfigurationStorageProvider> tabConfigurationStorageProvider;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logger = new Mock<ILogger<TabConfigurationController>>();
#pragma warning disable CS0618 // Type or member is obsolete
            this.telemetryClient = new TelemetryClient();
#pragma warning restore CS0618 // Type or member is obsolete
            this.tabConfigurationStorageProvider = new Mock<ITabConfigurationStorageProvider>();

            this.tabConfigurationController = new TabConfigurationController(
                this.logger.Object,
                this.telemetryClient,
                this.tabConfigurationStorageProvider.Object);

            this.tabConfigurationController.ControllerContext = new ControllerContext();
            this.tabConfigurationController.ControllerContext.HttpContext =
                FakeHttpContext.GetMockHttpContextWithUserClaims();
        }

        /// <summary>
        /// Test case to check if tab configuration data is not valid.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task PostAsyncNotValidContentsAsync()
        {
            // ARRANGE
            this.tabConfigurationStorageProvider
                .Setup(x => x.UpsertTabConfigurationDetailAsync(TabConfigurationData.TabConfigurationEntity))
                .Returns(Task.FromResult(true));

            // ACT
            var result = (ObjectResult)await this.tabConfigurationController.PostAsync(TabConfigurationData.TeamId, TabConfigurationData.GroupId).ConfigureAwait(false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Test case to check if team id is passing as empty.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task PostAsyncTeamIdEmptyCheckAsync()
        {
            // ARRANGE
            var teamId = string.Empty;

            // ACT
            var result = (ObjectResult)await this.tabConfigurationController.PostAsync(teamId, TabConfigurationData.GroupId).ConfigureAwait(false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Test case to check if team id is passing as null.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task PostAsyncTeamIdNullCheckAsync()
        {
            // ACT
            var result = (ObjectResult)await this.tabConfigurationController.PostAsync(null, TabConfigurationData.GroupId).ConfigureAwait(false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
        }
    }
}