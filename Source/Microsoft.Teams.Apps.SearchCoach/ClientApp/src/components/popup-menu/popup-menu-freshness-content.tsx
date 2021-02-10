// <copyright file="popup-menu-freshness-content.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { useTranslation } from "react-i18next";
import { Button, Menu, MenuButton, MenuItem } from "@fluentui/react-northstar";
import resources from "../../constants/resources";

import "./popup-menu.css";

interface IPopUpFreshnessContentProps {
    onFreshnessChange: (value: string) => void;
}

/** 
* This component contains freshness filter popup content.
* @param props {IPopUpFreshnessContentProps} The props for this component.
*/
const PopUpFreshnessContent: React.FunctionComponent<IPopUpFreshnessContentProps> = (props: IPopUpFreshnessContentProps) => {

    const [popup, setPopUp] = React.useState(false);
    const { t } = useTranslation();

    /**
    * The function checks the selected value of freshness and returns a key value.
    * It is used to omit the passing of UI facing strings to back-end.
    * @param selectedValue {String} selected freshness value.
    */
    const checkSelectedValue = (selectedValue: string) => {
        let freshnessKey = "";

        switch (selectedValue) {

            case t("past24hoursLabelText"):
                freshnessKey = resources.freshnessDayText;
                break;
            case t("pastWeekLabelText"):
                freshnessKey = resources.freshnessWeekText;
                break;
            case t("pastMonthLabelText"):
                freshnessKey = resources.freshnessMonthText;
                break;
            /**
            * Here we have made "Month" as the default freshness value.
            * If the user selects for "Anytime" too, we would consider "Month" as the selected value.
            */
            default:
                freshnessKey = resources.freshnessMonthText;
                break;
        }

        return freshnessKey;
    }

    /**
    * The event handler click of freshness value.
    * @param event {any} Event object for click.
    */
    const handleClick = (event: any) => {
        const freshnessKeyValue = checkSelectedValue(event.currentTarget.innerText);
        props.onFreshnessChange(freshnessKeyValue);
        setPopUp(false);
    };

    return (
        <MenuButton
            trigger={<Button content={t("anyTimeButton")} text size="small" />}
            onOpenChange={({ open }: any) => setPopUp(open)}
            menu={
                <Menu id="freshness-menu" vertical pointing="start">
                    <MenuItem onClick={handleClick}>{t("anyTimeLabelText")}</MenuItem>
                    <MenuItem onClick={handleClick}>{t("past24hoursLabelText")}</MenuItem>
                    <MenuItem onClick={handleClick}>{t("pastWeekLabelText")}</MenuItem>
                    <MenuItem onClick={handleClick}>{t("pastMonthLabelText")}</MenuItem>
                </Menu>
            }
            open={popup}
        />
    );
}

export default PopUpFreshnessContent