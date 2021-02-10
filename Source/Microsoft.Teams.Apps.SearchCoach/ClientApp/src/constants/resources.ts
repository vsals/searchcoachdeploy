// <copyright file="resources.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import { IDropdownListItem } from "models/IDropdownFilter";

export default class Resources {

    // themes
    public static readonly body: string = "body";
    public static readonly theme: string = "theme";
    public static readonly default: string = "default";
    public static readonly light: string = "light";
    public static readonly dark: string = "dark";
    public static readonly contrast: string = "contrast";

    // screen size
    public static readonly screenWidthLarge: number = 1200;
    public static readonly screenWidthSmall: number = 1000;

    // Countries list to be shown in location filter
    public static readonly countries: IDropdownListItem[] = [
        { name: "No Filter", id: "nf" } as IDropdownListItem,
        { name: "United States", id: "en-US" } as IDropdownListItem,
        { name: "Japan", id: "ja-JP" } as IDropdownListItem,
        { name: "France", id: "fr-FR" } as IDropdownListItem,
        { name: "Germany", id: "de-DE" } as IDropdownListItem,
        { name: "Italy", id: "it-IT" } as IDropdownListItem,
        { name: "South Korea", id: "ko-KR" } as IDropdownListItem,
        { name: "Russia", id: "ru-RU" } as IDropdownListItem
    ];

    // Domains list to be shown in domain filter
    public static readonly domains: IDropdownListItem[] = [
        { name: ".com", id: "1" } as IDropdownListItem,
        { name: ".org", id: "2" } as IDropdownListItem,
        { name: ".edu", id: "3" } as IDropdownListItem,
        { name: ".net", id: "4" } as IDropdownListItem,
        { name: ".gov", id: "5" } as IDropdownListItem,
        { name: ".mil", id: "6" } as IDropdownListItem,
    ];

    // Filter buttons
    public static readonly operatorText: string = "operators";
    public static readonly locationText: string = "location";
    public static readonly domainText: string = "alldomains";
    public static readonly anytimeText: string = "anytime";

    // Marking unknown category or country
    public static readonly unknownText: string = "Unknown";

    // Operator filters
    public static readonly andOperatorText: string = "AND";
    public static readonly orOperatorText: string = "OR";
    public static readonly notOperatorText: string = "NOT";

    // Freshness values
    public static readonly freshnessDayText: string = "Day";
    public static readonly freshnessWeekText: string = "Week";
    public static readonly freshnessMonthText: string = "Month";

    // Average network timeout in milliseconds.
    public static readonly axiosDefaultTimeout: number = 10000;

    // Leader-board table header key.
    public static readonly leaderBoardTableHeaderKey: string = "header";
}