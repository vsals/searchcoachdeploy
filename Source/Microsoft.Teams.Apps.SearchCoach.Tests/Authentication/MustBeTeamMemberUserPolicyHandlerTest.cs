﻿// <copyright file="MustBeTeamMemberUserPolicyHandlerTest.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Authentication
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Authentication.AuthenticationPolicy;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.GroupMembers;
    using Microsoft.Teams.Apps.SearchCoach.Tests.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Test class for Team member policy handler
    /// </summary>
    [TestClass]
    public class MustBeTeamMemberUserPolicyHandlerTest
    {
        /// <summary>
        /// Instance of Mocked MemberValidationService to validate member.
        /// </summary>
        private Mock<IMemberValidationService> memberValidationService;

        /// <summary>
        /// Instance of IOptions to read team's group data from azure application configuration.
        /// </summary>
        private IOptions<BotSettings> botSettings;

        // Instance of MustBeTeamMemberUserPolicyHandler for policy check.
        private MustBeTeamMemberUserPolicyHandler policyHandler;

        // Instance of AuthorizationHandlerContext for authorization context.
        private AuthorizationHandlerContext authContext;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.memberValidationService = new Mock<IMemberValidationService>();
            this.botSettings = Options.Create(new BotSettings()
            {
                MicrosoftAppId = "xxxx-xxxx-xxxx",
                AppBasePath = "https://foo",
            });
        }

        /// <summary>
        /// Validate Team member user policy handler.
        /// </summary>
        /// <returns>A <see cref="Task"/> Representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateHandleSucceedAsync()
        {
            // Arrange
            this.authContext = FakeHttpContext.GetAuthorizationHandlerContextForTeamMember();
            IMemoryCache memoryCache = new FakeMemoryCache();

            this.policyHandler = new MustBeTeamMemberUserPolicyHandler(
                memoryCache,
                this.botSettings,
                this.memberValidationService.Object);

            // Act
            await this.policyHandler.HandleAsync(this.authContext);

            // Assert
            Assert.IsTrue(this.authContext.HasSucceeded);
        }

        /// <summary>
        /// Validate Team member user policy handler.
        /// </summary>
        /// <returns>A <see cref="Task"/> Representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateHandleFailedAsync()
        {
            // Arrange
            this.authContext = FakeHttpContext.GetAuthorizationHandlerContextForTeamMember();
            var mockMemoryCache = new Mock<IMemoryCache>();

            mockMemoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            this.policyHandler = new MustBeTeamMemberUserPolicyHandler(
                mockMemoryCache.Object,
                this.botSettings,
                this.memberValidationService.Object);

            this.memberValidationService
                .Setup(svc => svc.ValidateMemberAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => false);

            // Act
            await this.policyHandler.HandleAsync(this.authContext);

            // Assert
            Assert.IsFalse(this.authContext.HasSucceeded);
        }
    }
}