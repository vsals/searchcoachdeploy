// <copyright file="LeaderBoardControllerTests.cs" company="Microsoft">
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
    using Microsoft.Teams.Apps.SearchCoach.Models;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.Users;
    using Microsoft.Teams.Apps.SearchCoach.Tests.Fakes;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Test class that contains test cases related to leader board controller methods.
    /// </summary>
    [TestClass]
    public class LeaderBoardControllerTests
    {
        private Mock<ILogger<LeaderBoardController>> logger;
        private TelemetryClient telemetryClient;
        private LeaderBoardController leaderBoardController;
        private Mock<IUsersService> usersService;
        private Mock<IUserResponseStorageProvider> userResponseStorageProvider;
        private Mock<ITabConfigurationStorageProvider> tabConfigurationStorageProvider;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logger = new Mock<ILogger<LeaderBoardController>>();
#pragma warning disable CS0618 // Type or member is obsolete
            this.telemetryClient = new TelemetryClient();
#pragma warning restore CS0618 // Type or member is obsolete
            this.usersService = new Mock<IUsersService>();
            this.userResponseStorageProvider = new Mock<IUserResponseStorageProvider>();
            this.tabConfigurationStorageProvider = new Mock<ITabConfigurationStorageProvider>();

            this.leaderBoardController = new LeaderBoardController(
                this.logger.Object,
                this.telemetryClient,
                this.userResponseStorageProvider.Object,
                this.usersService.Object,
                this.tabConfigurationStorageProvider.Object);

            this.leaderBoardController.ControllerContext = new ControllerContext();
            this.leaderBoardController.ControllerContext.HttpContext =
                FakeHttpContext.GetMockHttpContextWithUserClaims();
        }

        /// <summary>
        /// Test case to check if user responses data is not null and valid.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task UserResponsesNotNullValidContentsAsync()
        {
            // ARRANGE
            var userResponseEntities = LeaderBoardData.UserResponseEntities;

            this.userResponseStorageProvider
                .Setup(x => x.GetUsersResponsesAsync(LeaderBoardData.TeamId, LeaderBoardData.GroupId))
                .Returns(Task.FromResult(LeaderBoardData.UserResponseEntities.AsEnumerable()));

            this.usersService
                .Setup(x => x.GetUserDisplayNamesAsync(LeaderBoardData.UserId, "fake_token", LeaderBoardData.UserObjectIds))
                .Returns(Task.FromResult(LeaderBoardData.UsersDetails));

            this.tabConfigurationStorageProvider
                .Setup(x => x.GetTabConfigurationEntityAsync(LeaderBoardData.TeamId, LeaderBoardData.TabId))
                .Returns(Task.FromResult(LeaderBoardData.TabConfigurationDetails));

            // ACT
            var result = (ObjectResult)await this.leaderBoardController.GetUsersResponsesAsync(
                LeaderBoardData.TeamId,
                LeaderBoardData.TabId,
                LeaderBoardData.GroupId).ConfigureAwait(false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);

            Assert.AreEqual(
                ((IEnumerable<UserResponseDataModel>)result.Value).Count(),
                LeaderBoardData.UserResponseEntities.Count);
        }

        /// <summary>
        /// Test case to check if team id is passing as empty.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task UserResponsesTeamIdEmptyCheckAsync()
        {
            // ARRANGE
            var teamId = string.Empty;

            // ACT
            var result = (ObjectResult)await this.leaderBoardController.GetUsersResponsesAsync(
                teamId,
                LeaderBoardData.TabId,
                LeaderBoardData.GroupId).ConfigureAwait(false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Test case to check if team id is passing as null.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task UserResponsesTeamIdNullCheckAsync()
        {
            // ACT
            var result = (ObjectResult)await this.leaderBoardController.GetUsersResponsesAsync(
                teamId: null,
                LeaderBoardData.TabId,
                LeaderBoardData.GroupId).ConfigureAwait(false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Test case to check if team's group id is passing as empty.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task UserResponsesGroupIdEmptyCheckAsync()
        {
            // ARRANGE
            var groupId = string.Empty;

            // ACT
            var result = (ObjectResult)await this.leaderBoardController.GetUsersResponsesAsync(
                LeaderBoardData.TeamId,
                LeaderBoardData.TabId,
                groupId).ConfigureAwait(false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Test case to check if team's group id is passing as null.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task UserResponsesGroupIdNullCheckAsync()
        {
            // ACT
            var result = (ObjectResult)await this.leaderBoardController.GetUsersResponsesAsync(
                LeaderBoardData.TeamId,
                LeaderBoardData.TabId,
                groupId: null).ConfigureAwait(false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status404NotFound);
        }
    }
}