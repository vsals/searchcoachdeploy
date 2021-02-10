// <copyright file="IDropdownFilter.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

/**
* Interface to work with drop-down list item values.
*/
export interface IDropdownListItem {
    name: string;
    id: string;
}

/**
* Interface to work with drop-down list selected item value.
*/
export interface ISelectedDropdownItem {
    header: string,
    key: string,
}