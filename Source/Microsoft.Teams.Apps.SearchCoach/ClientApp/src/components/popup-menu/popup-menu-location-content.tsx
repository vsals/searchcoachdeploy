// <copyright file="popup-menu-location-content.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import React from "react";
import { useTranslation } from "react-i18next";
import { Text, Flex, Dropdown } from "@fluentui/react-northstar";
import resources from "../../constants/resources";
import { IDropdownListItem, ISelectedDropdownItem } from "../../models/IDropdownFilter";

import "./popup-menu.css";

interface IPopUpLocationContentProps {
    onCountryChange: (value: ISelectedDropdownItem) => void;
    selectedCountry: ISelectedDropdownItem;
}

/** 
* This component contains location filter popup content.
* @param props {IPopUpLocationContentProps} The props for this component.
*/
const PopUpLocationContent: React.FunctionComponent<IPopUpLocationContentProps> = (props: IPopUpLocationContentProps) => {

    const dropdownItems = resources.countries;
    const { t } = useTranslation();
    const initialSelectedDropdownValue: ISelectedDropdownItem = { header: resources.countries[0].name, key: resources.countries[0].id };
    const [selectedCountry, setSelectedCountry] = React.useState(props.selectedCountry === null ? initialSelectedDropdownValue : props.selectedCountry);

    // This event handlers handles location drop-down value change.
    const onLocationValueChange = (event: any, dropdownProps?: any) => {
        props.onCountryChange(dropdownProps.value);
        setSelectedCountry(dropdownProps.value);
        return "";
    };

    return (
        <div className="location-popup">
            <Flex gap="gap.smaller">
                <Text size="small" content={"Search"} />
            </Flex>
            <Flex gap="gap.smaller" className="search-dropdown-margin">
                <Dropdown
                    className="location-dropdown"
                    items={dropdownItems.map((value: IDropdownListItem) => ({ key: value.id, header: value.name }))}
                    value={selectedCountry.header}
                    placeholder={t("dropdownPlaceholder")}
                    onChange={onLocationValueChange}
                />
            </Flex>
            <Flex className="margin-top">
                <Text content={t("countryHelpTitle")} weight="semibold" />
            </Flex>
            <Flex>
                <Text content={t("countryHelpContent")} />
            </Flex>
        </div>
    );
}

export default PopUpLocationContent