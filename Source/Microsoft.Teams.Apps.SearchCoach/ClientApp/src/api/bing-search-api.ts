// <copyright file="bing-search-api.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import axiosDecorator from "../api/axios-decorator";
import { AxiosResponse } from "axios";
import { ISearchFilterModel } from "../models/ISearchFilterModel"

/**
* Get bing search results.
* @param searchFilterRequestDetails {ISearchFilterModel } filters selected by user
*/
export const getBingSearchResults = async (
    searchFilterRequestDetails: ISearchFilterModel): Promise<AxiosResponse> => {

    const url = `/api/search`;

    return await axiosDecorator.post(url, searchFilterRequestDetails);
}