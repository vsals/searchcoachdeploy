// <copyright file="tab-configuration-api.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import axios from "./axios-decorator";
import { ITabConfiguration } from "../models/ITabConfiguration";
import { AxiosResponse } from "axios";

/**
* Save team's tab configuration details in storage.
* @param teamId {String} team id of configurable tab.
* @param groupId {String} group id of current team.
*/
export const createTabConfiguration = async (teamId: string, groupId: string): Promise<AxiosResponse<ITabConfiguration>> => {
    let url = `/api/tabconfiguration/${teamId}?groupId=${groupId}`;
    return await axios.post(url);
}