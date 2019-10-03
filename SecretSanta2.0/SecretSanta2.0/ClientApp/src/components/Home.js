import { getSignalRConnection, storeSignalRConnection } from '../helpers/signalRHelper';
import { UserIsValid, TryGetToken } from '../helpers/authHelper';
import { withRouter, Link, Redirect } from 'react-router-dom';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import config from '../config.json';
import Cookies from 'js-cookie';

class Home extends Component {
	static displayName = Home.name;
	
	constructor(props) {
		super(props);
		this.state = {
            participantNames: [],
            hubConnection: null,
			secretSanta: [ '', '', '', '', '', '', ],
			selectedName: '',
			validationError: '',
			status: 0,
        };
        this._ismounted = false;
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
			event.target.myButton.value = 'Please wait...';
			this.postForm(event);
		}
		else {
			this.setState({ status: 3 });
		}
	}

	postForm(event) {
		let inputName = event.target.FirstName.value;	

		if (!inputName || inputName === '' || inputName === 'Please Select Your Name') {
			event.target.myButton.disabled = false;
			this.setState({ validationError: "Please select your name before clicking 'Draw Name'." });
			return false;
		}

		fetch(config.GET_SECRET_SANTA_URL + '?name=' + inputName, {
			method: 'POST',
			headers: {
				'Authorization': TryGetToken(this.props.auth.user)
			},
		})
		.then((response) => {
			return response.json();
		})
		.then(data => {
			this.setState({
				secretSanta: [
					data.title,
					data.pageDescription,
					data.headerOne,
					data.name,
					data.headerTwo,
					data.wishList,
				],
				status: data.response
			});
		})
		.catch(error => {
			console.error(error);
			document.getElementById("myButton").disabled = false;
			document.getElementById("myButton").value = 'Draw Name';
			alert('There was an issue when you logged in. Please logout and try again.');
		});
	}

	render() {
		return (
			<div>
				{this.state.status === 0 ? (
					<form onSubmit={this.checkForm}>
						<div className="form-horizontal">
							<h2>The 2019 Christmas Season Secret Santa!</h2>

							<br />

							{UserIsValid(this.props.auth) && Cookies.get('User-Email') != null ? (<p>Welcome, <strong>{Cookies.get('User-Email')}</strong>!</p>) : ('')}
							<p>Please select your name from the dropdown so that you don't accidentally draw yourself, and then click on <strong>Draw Name</strong> to randomly pick someone from your friend group.</p>
							<p>If you don't see your name in the dropdown, please click <strong>Join the Fun</strong> in the header, or just click <Link to="/join-the-fun">this link</Link>.</p>

							<hr />

							<div className="form-group">
								<label className="control-label col-md-2">Your Name:</label>
								<div className="col-md-10">
									<select
										className="form-control user-input"
										id="FirstName"
										name="FirstName"
										value={this.state.selectedName}
										onChange={(e) => this.setState({
											selectedName: e.target.value,
											validationError: e.target.value === "" ? "Please select your name before clicking 'Draw Name'." : ""
										})}>
										{this.state.participantNames.map((name) => <option key={name.value} value={name.value}>{name.display}</option>)}
									</select>
									<div style={{ color: 'red', marginTop: '5px' }}>
										{this.state.validationError}
									</div>
								</div>
							</div>

							<div className="form-group">
								<div className="col-md-offset-2 col-md-10">
									<input type="submit" value="Draw Name" name="myButton" id="myButton" className="btn btn-default christmas-green" />
								</div>
							</div>
						</div>
					</form>
				) : ('')}
				{this.state.status === 1 || this.state.status === 2 ? (
						<div className="form-horizontal">
						<h2>{this.state.secretSanta[0]}</h2>
						<p>{this.state.secretSanta[1]}</p>

						<table className="table">
							<tbody>
								{this.state.secretSanta[2] ?
									<tr>
										<td>
											<b>{this.state.secretSanta[2]}</b>
										</td>
									</tr> :
									<tr>

									</tr>
								}
								
								{this.state.secretSanta[3] ?
									<tr>
										<td>
											{this.state.secretSanta[3]}
										</td>
									</tr> :
									<tr>

									</tr>
								}

								{this.state.secretSanta[4] ?
									<tr>
										<td>
											<b>{this.state.secretSanta[4]}</b>
										</td>
									</tr> :
									<tr>

									</tr>
								}

								<tr>
									<td>
										{this.state.secretSanta[5]}
									</td>
								</tr>
							</tbody>
						</table>

						<center>
							<button className="btn btn-primary hidden-print christmas-green" onClick={this.printPage}><span className="glyphicon glyphicon-print" aria-hidden="true"></span> Print</button>
						</center>
					</div>
				) : ('')}
				{this.state.status === 3 ? (
					<Redirect
						to={{
							pathname: '/login',
							state:
							{
								from: this.props.location
							}
						}}
					/>
				) : ('')}
			</div>
		);
	}
    
    componentDidMount = () => {
        this._ismounted = true;
        getSignalRConnection(this.props.signalR, config.SIGNALR_SANTA_HUB)
            .then((conn) => {
                this.props.storeSignalRConnection(conn);
                conn.on(config.SIGNALR_SANTA_HUB_GET_PARTICIPANTS, (data) => {
                    if (this._ismounted) {
                        let participants = data.participants.map(name => { return { value: name, display: name } })
                        this.setState({ participantNames: [{ value: '', display: 'Please Select Your Name' }].concat(participants) });
                    }
                })
                conn.invoke(config.SIGNALR_SANTA_HUB_GET_PARTICIPANTS);
            })
            .catch(error => console.error(error));
    }

    componentWillUnmount() {
        this._ismounted = false;
    }
};

const mapStateToProps = (state) => {
	return {
        auth: state.auth,
        signalR: state.signalR
	};
};

const mapDispatchToProps = (dispatch) => {
    return {
        storeSignalRConnection: (url) => {
            dispatch(storeSignalRConnection(url));
        }
    }
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Home));
