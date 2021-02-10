// <copyright file="popup-menu-domain-content.test.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { Provider } from "@fluentui/react-northstar";
import PopUpDomainContent from "../popup-menu-domain-content";
import { render, unmountComponentAtNode } from "react-dom";
import { act } from "react-dom/test-utils";
import pretty from "pretty";
import resources from "../../../constants/resources";
import { ISelectedDomainValue } from "../../../models/ISelectedDomainValue";
import { IDropdownListItem } from "../../../models/IDropdownFilter";

jest.mock("react-i18next", () => ({
    useTranslation: () => ({
        t: (key: any) => key,
        i18n: { changeLanguage: jest.fn() },
    }),

    withTranslation: () => (Component: any) => {
        Component.defaultProps = {
            ...Component.defaultProps,
            t: (key: any) => key,
        };
        return Component;
    },
}));

jest.mock("@microsoft/teams-js", () => ({
    initialize: () => {
        return true;
    },
    getContext: (callback: any) =>
        callback(
            Promise.resolve({ teamId: "ewe", entityId: "sdsd", locale: "en-US" })
        ),
}));

let container: any = null;
let initialDomainValuesList: ISelectedDomainValue[] = [];
initialDomainValuesList = resources.domains.map((value: IDropdownListItem) => ({ key: value.id, title: value.name, isChecked: false }));

beforeEach(() => {
    // setup a DOM element as a render target.
    container = document.createElement("div");
    // container *must* be attached to document so events work correctly.
    document.body.appendChild(container);
});

afterEach(() => {
    // cleanup on exiting.
    unmountComponentAtNode(container);
    container.remove();
    container = null;
});

describe("PopUpDomainContent", () => {
    it("renders snapshots", () => {
        act(() => {
            render(
                <Provider>
                    <PopUpDomainContent
                        onDomainValueChange={(selectedDomains: ISelectedDomainValue[]) => { }}
                        selectedDomainValuesList={initialDomainValuesList}
                    />
                </Provider>,
                container
            );
        });

        expect(pretty(container.innerHTML)).toMatchSnapshot();
    });
});