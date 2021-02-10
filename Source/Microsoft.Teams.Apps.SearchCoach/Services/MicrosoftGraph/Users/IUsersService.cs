﻿// <copyright file="IUsersService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.Users
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface that helps to get the user's data from Graph API.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Get users information from graph API.
        /// </summary>
        /// <param name="loggedInUserObjectId">Azure AD user id of the signed in user</param>
        /// <param name="authorizationHeader">Authorization header value. Usually this value will be provided in HTTP context of signed in user.</param>
        /// <param name="userObjectIds">Collection of AAD Object ids of users.</param>
        /// <returns>Returns user id and name key value pairs.</returns>
        Task<Dictionary<Guid, string>> GetUserDisplayNamesAsync(
            string loggedInUserObjectId,
            string authorizationHeader,
            IEnumerable<string> userObjectIds);
    }
}