// <copyright file="config.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import React from 'react';
import * as microsoftTeams from "@microsoft/teams-js";
import { getBaseUrl } from '../configVariables';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { createTabConfiguration } from "../api/tab-configuration-api";

// Class that handles configuration tab actions.
class Configuration extends React.Component<WithTranslation> {
    localize: TFunction;
    groupId: string;
    teamId: string;

    constructor(props: WithTranslation) {
        super(props);
        this.localize = this.props.t;
        this.groupId = "";
        this.teamId = "";
    }

    public componentDidMount() {
        microsoftTeams.initialize();

        microsoftTeams.getContext(async (context: microsoftTeams.Context) => {
            this.groupId = context.groupId!;
            this.teamId = context.teamId!;
        });

        microsoftTeams.settings.registerOnSaveHandler(async (saveEvent: microsoftTeams.settings.SaveEvent) => {

            let response = await createTabConfiguration(this.teamId, this.groupId);
            if (response.status === 200 && response.data) {
                microsoftTeams.settings.setSettings({
                    entityId: response.data.tabId!,
                    contentUrl: getBaseUrl() + "/leader-board-tab",
                    suggestedDisplayName: this.localize("suggestedTabDisplayName"),
                });

                saveEvent.notifySuccess();
            } else {
                saveEvent.notifyFailure();
            }
        });

        microsoftTeams.settings.setValidityState(true);
    }

    public render(): JSX.Element {
        return (
            <div className="configContainer">
                <h3>{this.localize("configContainerSaveText")}</h3>
            </div>
        );
    }
}

export default withTranslation()(Configuration);