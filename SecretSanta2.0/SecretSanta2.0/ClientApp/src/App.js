import NavigationBar from './components/NavigationBar';
import { Route, Switch } from 'react-router-dom';
import JoinTheFun from './components/JoinTheFun';
import Logout from './components/Logout';
import React, { Component } from 'react';
import Login from './components/Login';
import Home from './components/Home';

const NotFound = () => (
	<div className="form-horizontal">
		<h2>Not Found!</h2>

		<br />

		<p>The page you have requested does not exist. :(</p>
	</div>
)

class App extends Component {
  static displayName = "Trevor's Secret Santa";

	render () {
		return (
			<div>
				<NavigationBar />
				<main role="main" className="container">
					<Switch>
						<Route exact path='/' component={(props) => (<Home {...props} />)} />
						<Route path='/join-the-fun' component={(props) => (<JoinTheFun {...props} />)} />
						<Route path='/login' component={(props) => (<Login {...props} />)} />
						<Route path='/logout' component={(props) => (<Logout {...props} />)} />
						<Route component={NotFound} />
					</Switch>
				</main>
			</div>
		);
	}
};

export default App;