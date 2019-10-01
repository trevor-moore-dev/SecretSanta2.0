import React, { Component } from 'react';
import { GoogleLogin } from 'react-google-login';
import { connect } from "react-redux";
import { login } from "../actions/authActions";
import { UserIsValid } from "../services/authService";
import config from '../config.json';
import { withRouter, Redirect } from "react-router-dom";

class Login extends Component {

	onFailure = (error) => {
		console.error(error);
	};

	googleResponse = (response) => {
		if (!response.tokenId) {
			console.error("Unable to get tokenId from Google", response)
			return;
		}

		const tokenBlob = new Blob([JSON.stringify({ tokenId: response.tokenId }, null, 2)], { type: 'application/json' });
		const options = {
			method: 'POST',
			body: tokenBlob,
			mode: 'cors',
			cache: 'default'
		};
		fetch(config.GOOGLE_AUTH_CALLBACK_URL, options)
		.then((response) => {
			return response.json();
		})
		.then(data => {
			const token = data.token;
			this.props.login(token);
		})
		.catch(error => {
			console.error(error);
			alert('An error occurred while logging you in. Please try again.');
		});
	};

	render() {
		let content = UserIsValid(this.props.auth) ?
		(
			<div>
				<Redirect to={{
					pathname: this.props.location.state == null ? '/' : this.props.location.state.from.pathname
				}} />
			</div>
		) : (
			<div>
				<GoogleLogin
				clientId={config.GOOGLE_CLIENT_ID}
				buttonText="Google Login"
				onSuccess={this.googleResponse}
				onFailure={this.onFailure}
				/>
			</div>
		);

		return (
			<div className="form-horizontal">

				<h2>Login to Join the Fun!</h2>

				<br />

				<p>Please login using your <strong>Google</strong> account to continue.</p>

				<hr />

				{content}

			</div>
		);
	}
};

const mapStateToProps = (state) => {
	return {
		auth: state.auth
	};
};

const mapDispatchToProps = (dispatch) => {
	return {
		login: (token) => {
			dispatch(login(token));
		}
	}
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Login));
