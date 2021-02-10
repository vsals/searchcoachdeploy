// <copyright file="app.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { AppRoute } from "router/router";
import resources from "constants/resources";
import * as microsoftTeams from "@microsoft/teams-js";
import { Provider, teamsDarkTheme, teamsHighContrastTheme, teamsTheme } from "@fluentui/react-northstar";
import i18n from "i18n";

import "styles/site.css";

export interface IAppState {
	theme: string;
}

export default class App extends React.Component<{}, IAppState> {
	theme?: string | null;

	constructor(props: any) {
		super(props);
		let search = window.location.search;
		this.theme = new URLSearchParams(search).get("theme");

		this.state = {
			theme: this.theme ? this.theme : resources.default,
		}
	}

	componentDidMount() {
		microsoftTeams.initialize();
		microsoftTeams.getContext((context: microsoftTeams.Context) => {
			this.setState({ theme: context.theme! });
			i18n.changeLanguage(context.locale);
		});

		microsoftTeams.registerOnThemeChangeHandler((theme: string) => {
			this.setState({ theme: theme! }, () => {
				this.forceUpdate();
			});
		});
	}

	public setThemeComponent = () => {
		if (this.state.theme === resources.dark) {
			return (
				<Provider theme={teamsDarkTheme}>
					<div className="dark-container">
						{this.getAppDom()}
					</div>
				</Provider>
			);
		}
		else if (this.state.theme === resources.contrast) {
			return (
				<Provider theme={teamsHighContrastTheme}>
					<div className="high-contrast-container">
						{this.getAppDom()}
					</div>
				</Provider>
			);
		} else {
			return (
				<Provider theme={teamsTheme}>
					<div className="default-container">
						{this.getAppDom()}
					</div>
				</Provider>
			);
		}
	}

	public getAppDom = () => {
		return (
			<div className="appContainer">
				<AppRoute />
			</div>);
	}

	/**
	* Renders the component.
	*/
	public render(): JSX.Element {
		return (
			<div>
				{this.setThemeComponent()}
			</div>
		);
	}
}