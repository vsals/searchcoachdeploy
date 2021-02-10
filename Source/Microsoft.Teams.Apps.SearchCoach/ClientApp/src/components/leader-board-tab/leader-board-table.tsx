﻿// <copyright file="leader-board-table.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import React from 'react';
import { Table, Text } from '@fluentui/react-northstar';
import { useTranslation } from 'react-i18next';
import { ILeaderBoardUserData } from "../../models/ILeaderBoardUserData";
import Resources from '../../constants/resources';

import "../../components/leader-board-tab/leader-board-tab.css";

// Interface to work with leader-board user's responses data.
interface ILeaderBoardUserTableProps {
    userResponsesData: Array<ILeaderBoardUserData>
}

// Component for user responses table details.
const LeaderBoardUserTable: React.FunctionComponent<ILeaderBoardUserTableProps> = (props: ILeaderBoardUserTableProps) => {
    const localize = useTranslation().t;

    // Set table headers for leader-board tab.
    const leaderBoardTableHeader = {
        key: Resources.leaderBoardTableHeaderKey,
        items: [
            {
                content: <Text content={localize("studentNameColumnHeaderText")} className="student-name-header" />,
                key: "studentName",
                className: "leader-board-column-header"
            },
            {
                content: <Text content={localize("correctAnswerColumnHeaderText")} />,
                key: "correctAnswer",
                className: "leader-board-column-header"
            },
            {
                content: <Text content={localize("questionsAttemptedColumnHeaderText")} />,
                key: "questionsAttempted",
                className: "leader-board-column-header"
            }
        ],
    }

    // Set table rows for leader-board tab.
    let leaderBoardTableRows = props.userResponsesData.map((value: any, index: number) => (
        {
            key: index,
            items: [
                {
                    content: <Text content={value.userName} title={value.userName} className="user-name" />,
                    key: index + "1",
                    truncateContent: true,
                    className: "user-response-name"
                },
                {
                    content: <Text content={value.rightAnswers} title={value.rightAnswers} />,
                    key: index + "2",
                    className: "user-response-data-count"
                },
                {
                    content: <Text content={value.questionsAttempted} title={value.questionsAttempted} />,
                    key: index + "3",
                    className: "user-response-data-count"
                }
            ],
        }
    ));

    return (
        <div>
            <Table
                header={leaderBoardTableHeader}
                rows={leaderBoardTableRows}
                className="user-data-table"
                data-testid="leader-board-table-data" />
        </div>
    );
}

export default LeaderBoardUserTable