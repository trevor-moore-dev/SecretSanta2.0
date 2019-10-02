﻿import React, { Component } from 'react';
import { connect } from 'react-redux';
import { withRouter, Link, Redirect } from 'react-router-dom';
import { UserIsValid, TryGetToken } from '../services/authService';
import { HubConnectionBuilder } from '@aspnet/signalr';
import Cookies from 'js-cookie';
import config from '../config.json';

class JoinTheFun extends Component {
	static displayName = JoinTheFun.name;

	constructor(props) {
		super(props);
		this.state = {
			names: [],
			inputName: '',
			inputWishlist: '',
			nameValidationError: '',
            wishlistValidationError: '',
            hubConnection: null,
			status: 0,
		};
		this.checkForm = this.checkForm.bind(this);
		this.postForm = this.postForm.bind(this);
		this.printPage = this.printPage.bind(this);
	}

	printPage() {
		window.print();
	}

	checkForm(event) {
		event.preventDefault();

		if (UserIsValid(this.props.auth)) {
			event.target.myButton.disabled = true;
			event.target.myButton.value = "Please wait...";
			this.postForm(event);
		}
		else {
			alert('There was an issue when you logged in. Please logout and try again.');
		}
	}

	postForm(event) {
		var nameValidation = '';
		var wishlistValidation = '';
		var validate = false;

		if (this.state.inputName === '') {
			event.target.myButton.disabled = false;
			nameValidation = 'Please enter your name.';
			validate = true;
		}

		if (this.state.inputWishlist === '') {
			event.target.myButton.disabled = false;
			wishlistValidation = 'Please enter your wishlist.';
			validate = true;
		}

		if (validate) {
			this.setState({
				nameValidationError: nameValidation,
				wishlistValidationError: wishlistValidation
			});
			return false;
		}

		fetch(config.JOIN_THE_FUN_URL, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
				'Authorization': TryGetToken(this.props.auth.user)
			},
			body: JSON.stringify({ Name: this.state.inputName, Wishlist: this.state.inputWishlist })
		})
		.then((response) => {
			return response.json();
		})
		.then(data => {
			var nameError = '';
			if (data === 2) {
				document.getElementById("myButton").disabled = false;
				nameError = 'Someone has already entered with that name. Please add an initial.';
			}
            this.setState({
                nameValidationError: nameError,
                status: data
            });
            this.state.hubConnection.invoke(config.SIGNALR_HUB_GET_PARTICIPANTS)
		})
		.catch(error => {
			console.error(error);
			document.getElementById("myButton").disabled = false;
			document.getElementById("myButton").value = 'Draw Name';
			alert('There was an issue when you logged in. Please logout and try again.');
		});
	}

	render() {
		const date = new Date();
		return (
			<div>
				{UserIsValid(this.props.auth) ? (
					<div>
						{this.state.status === 0 || this.state.status === 2 ? (
							<form onSubmit={this.checkForm}>
								<div className="form-horizontal">
									<h2>Participate in {date.getFullYear()}'s Secret Santa!</h2>

									<br />

									{Cookies.get('User-Email') == null ? (<p>Welcome!</p>) : (<p>Welcome, <strong>{Cookies.get('User-Email')}</strong>!</p>)}
									<p>Please enter your name and wishlist to join in on the fun this year!</p>

									<hr />

									<div className="form-group">
										<label className="control-label col-md-2 christmas-label">Your Name:</label>
										<div className="col-md-10">
											<input
												className="form-control text-box single-line user-input"
												id="Name"
												name="Name"
												placeholder="Santa Claus"
												type="text"
												value={this.state.inputName}
												maxLength="50"
												onChange={(e) => this.setState({ inputName: e.target.value.slice(0, 50) })} />
											<div style={{ color: 'red', marginTop: '5px' }}>
												{this.state.nameValidationError}
											</div>
										</div>
									</div>

									<div className="form-group">
										<label className="control-label col-md-2 christmas-label">Your Christmas Wishlist:</label>
										<div className="col-md-10">
											<textarea
												className="form-control user-input"
												cols="20"
												id="WishList"
												name="WishList"
												placeholder="I want a great big puppy!"
												rows="4"
												type="text"
												value={this.state.inputWishlist}
												maxLength="500"
												onChange={(e) => this.setState({ inputWishlist: e.target.value.slice(0, 500) })} />
											<div style={{ color: 'red', marginTop: '5px' }}>
												{this.state.wishlistValidationError}
											</div>
										</div>
									</div>

									<div className="form-group">
										<div className="col-md-offset-2 col-md-10">
											<input type="submit" value="Join" name="myButton" id="myButton" className="btn btn-default christmas-green" />
										</div>
									</div>
								</div>
							</form>
						) : (
								<div className="form-horizontal">
									<h2>Thanks for joining!</h2>

									<br />

									<p>You can now draw a name by visiting <Link to="/">this link</Link>.</p>
								</div>
							)}
					</div>
				) : (
						<Redirect
							to={{
								pathname: '/login',
								state:
								{
									from: this.props.location
								}
							}}
						/>
					)}
			</div>
		);
    }

    componentDidMount = () => {
        const connection = new HubConnectionBuilder()
            .withUrl(config.SIGNALR_HUB_GET_PARTICIPANTS_URL)
            .build();
        this.setState({ hubConnection: connection }, () => {
            this.state.hubConnection.start()
                .then(() => console.log('SignalR connection started!'))
                .catch(error => console.error(error));
        });
    }
};

const mapStateToProps = (state) => {
	return {
		auth: state.auth
	};
};

export default withRouter(connect(mapStateToProps)(JoinTheFun));