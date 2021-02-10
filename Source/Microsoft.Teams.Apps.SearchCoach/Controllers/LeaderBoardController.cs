// <copyright file="LeaderBoardController.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
namespace Microsoft.Teams.Apps.SearchCoach.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Authentication;
    using Microsoft.Teams.Apps.SearchCoach.Common;
    using Microsoft.Teams.Apps.SearchCoach.Models;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.Users;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeaderBoardController"/> class.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/leaderboard")]
    public class LeaderBoardController : BaseSearchCoachController
    {
        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provider to deal with user response provider methods.
        /// </summary>
        private readonly IUserResponseStorageProvider userResponseProvider;

        /// <summary>
        /// Instance of user service to get user's details.
        /// </summary>
        private readonly IUsersService usersService;

        /// <summary>
        /// Tab configuration provider to deal with configuration storage methods.
        /// </summary>
        private readonly ITabConfigurationStorageProvider tabConfigurationStorageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderBoardController"/> class.
        /// </summary>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="userResponseProvider">Provider for fetching user responses details from storage table.</param>
        /// <param name="usersService">Instance of user service to get user's details.</param>
        /// <param name="tabConfigurationStorageProvider">Tab configuration provider to deal with configuration storage methods.</param>
        public LeaderBoardController(
            ILogger<LeaderBoardController> logger,
            TelemetryClient telemetryClient,
            IUserResponseStorageProvider userResponseProvider,
            IUsersService usersService,
            ITabConfigurationStorageProvider tabConfigurationStorageProvider)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.userResponseProvider = userResponseProvider;
            this.usersService = usersService;
            this.tabConfigurationStorageProvider = tabConfigurationStorageProvider;
        }

        /// <summary>
        /// Get user's responses details from storage to show on leader-board tab.
        /// </summary>
        /// <param name="teamId">Team id to fetch user's responses details for that particular team.</param>
        /// <param name="tabId">Tab id of the configured tab in a team to fetch details.</param>
        /// <param name="groupId">Group id of the team to fetch team members for user validation.</param>
        /// <returns>A collection of user's responses details.</returns>
        [HttpGet("{teamId}/{tabId}")]
        [Authorize(PolicyNames.MustBeTeamMemberUserPolicy)]
        public async Task<IActionResult> GetUsersResponsesAsync(string teamId, string tabId, [FromQuery] string groupId)
        {
            try
            {
                this.logger.LogInformation("Leader-board user's responses - HTTP Get call initiated.");
                this.RecordEvent("Leader-board user's responses - HTTP Get call initiated.", RequestStatus.Initiated);

                // Get configured team's tab details.
                var tabConfiguration = await this.tabConfigurationStorageProvider.GetTabConfigurationEntityAsync(teamId, tabId);
                if (tabConfiguration == null)
                {
                    this.logger.LogError(StatusCodes.Status404NotFound, $"The tab configuration detail that user is trying to get does not exists for team Id: {teamId} and group Id: {groupId}.");
                    this.RecordEvent("Resource - HTTP Get call failed.", RequestStatus.Failed);
                    return this.NotFound($"No tab configuration detail found for team Id: {teamId} and group Id: {groupId}.");
                }

                // Check if team's tab is configured for currently passed group or not.
                if (tabConfiguration.GroupId != Guid.Parse(groupId))
                {
                    this.logger.LogError($"Leader-board user's responses - Team's tab is not configured for this group id: {groupId}.");
                    this.RecordEvent("Leader Board user's responses - HTTP Get call failed.", RequestStatus.Failed);
                    return this.BadRequest($"Leader-board user's responses - Team's tab is not configured for this group id: {groupId}.");
                }

                // Fetch user's responses details from storage for a particular team.
                var usersResponses = await this.userResponseProvider.GetUsersResponsesAsync(teamId, groupId);

                if (usersResponses == null)
                {
                    this.logger.LogInformation($"Leader-board user's responses not found for team: {teamId}");
                    this.RecordEvent($"Leader-board user's responses not found for team: {teamId}", RequestStatus.Failed);

                    return this.NotFound($"Leader-board user's responses not found for team: {teamId}");
                }

                // Get user's display names for all distinct users of the team.
                var idToNameMap = await this.usersService.GetUserDisplayNamesAsync(
                    this.UserAadId.ToString(),
                    this.Request?.Headers["Authorization"].ToString(),
                    usersResponses.Select(userResponse => userResponse.UserId.ToString()).Distinct());

                // Get user's responses for total correct answers and total attempted questions.
                var usersResponsesData = usersResponses
                    .GroupBy(userResponseEntity => userResponseEntity.UserId)
                    .Select(userResponse => new UserResponseDataModel()
                    {
                        RightAnswers = userResponse.Where(response => response.IsCorrectAnswer).Count(),
                        UserName = idToNameMap[userResponse.Key],
                        QuestionsAttempted = userResponse.Where(response => response.IsQuestionAttempted).Count(),
                    });

                this.logger.LogInformation("Leader-board user's responses - HTTP Get call succeeded.");
                this.RecordEvent("Leader-board user's responses - HTTP Get call succeeded.", RequestStatus.Succeeded);

                return this.Ok(usersResponsesData);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Leader-board user's responses - HTTP Get call failed for team Id: {teamId} and userId: {this.UserAadId}");
                this.RecordEvent("Leader-board user's responses - HTTP Get call failed.", RequestStatus.Failed);
                throw;
            }
        }
    }
}