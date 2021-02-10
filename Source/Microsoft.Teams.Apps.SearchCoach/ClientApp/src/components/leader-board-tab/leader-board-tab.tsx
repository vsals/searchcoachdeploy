﻿// <copyright file="leader-board-tab.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import * as microsoftTeams from '@microsoft/teams-js';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { getUserResponsesDetails } from "../../api/leader-board-api";
import { Text, Flex, Loader } from '@fluentui/react-northstar';
import LeaderBoardUserTable from "../leader-board-tab/leader-board-table";
import { ILeaderBoardUserData } from "../../models/ILeaderBoardUserData";

import "../../components/leader-board-tab/leader-board-tab.css";

// Interface for handling with leader-board state properties.
interface ILeaderBoardState {
    isLoading: boolean;
    userData: Array<ILeaderBoardUserData>;
    isValidRequest: boolean;
}

// Component for rendering user responses data on leader-board tab.
class LeaderBoardTab extends React.Component<WithTranslation, ILeaderBoardState> {
    localize: TFunction;
    monthList?: Array<string>;
    teamId: string;
    groupId: string;
    tabId: string;

    constructor(props: WithTranslation) {
        super(props);
        this.localize = this.props.t;
        this.teamId = "";
        this.groupId = "";
        this.tabId = "";
        this.state = {
            isLoading: true,
            userData: [],
            isValidRequest: true
        }
    }

    // Used to initialize Microsoft Teams sdk.
    componentDidMount() {
        microsoftTeams.initialize();
        microsoftTeams.getContext((context: microsoftTeams.Context) => {
            this.teamId = context.teamId!;
            this.groupId = context.groupId!;
            this.tabId = context.entityId!;
            this.getUsersResponses();
        });
    }

    // Fetch user's responses details to show on leader-board tab.
    private getUsersResponses = async () => {
        const response = await getUserResponsesDetails(this.teamId, this.groupId, this.tabId);

        if (response.status === 200 && response.data) {
            this.setState({
                userData: response.data,
                isLoading: false
            })
        } else if (response.status === 404 && response.data) {
            this.setState({
                userData: [],
                isLoading: false
            })
        } else {
            this.setState({
                userData: [],
                isValidRequest: false,
                isLoading: false
            })
        }
    }

    // Renders the component
    public render(): JSX.Element {
        return (
            <div>
                {
                    this.state.isLoading &&
                    <div className="container-div">
                        <div className="container-subdiv">
                            <div className="loader">
                                <Loader />
                            </div>
                        </div>
                    </div>
                }
                {
                    !this.state.isLoading && this.state.userData.length > 0 &&
                    <div>
                        <Flex>
                            <Text
                                className="leader-board-heading"
                                content={this.localize("leaderBoardTableHeaderText")}
                                weight="bold" />
                        </Flex>
                        <LeaderBoardUserTable userResponsesData={this.state.userData} />
                    </div>
                }
                {
                    !this.state.isLoading && this.state.isValidRequest && this.state.userData.length <= 0 &&
                    <div>
                        <Flex>
                            <Text
                                className="leader-board-error-heading"
                                content={this.localize("leaderBoardTabDataNotFoundText")}
                                weight="bold" />
                        </Flex>
                    </div>
                }
                {
                    !this.state.isLoading && !this.state.isValidRequest && this.state.userData.length <= 0 &&
                    <div>
                        <Flex>
                            <Text
                                className="leader-board-error-heading"
                                content={this.localize("leaderBoardTabBadRequestMessageText")}
                                weight="bold" color="red" />
                        </Flex>
                    </div>
                }
            </div>
        );
    }
}

export default withTranslation()(LeaderBoardTab)