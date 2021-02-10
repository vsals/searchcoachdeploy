// <copyright file="ISearchFilterModel.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

/** This interface is used for maintaining the state of domain check-box values. */
export interface ISearchFilterModel {
    // searched text by user.
    searchText: string;
    // selected domains using domain filter
    domains?: Array<string>;
    // selected freshness value using freshness filter
    freshness: string;
    // selected country-code using location filter
    market: string;
}