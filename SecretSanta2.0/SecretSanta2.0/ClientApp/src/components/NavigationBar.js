import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem } from 'reactstrap';
import { withRouter, NavLink, Link } from 'react-router-dom';
import { UserIsValid } from '../helpers/authHelper';
import logo from '../resources/images/Holly.png';
import '../resources/css/NavigationBar.css';
import React, { Component } from 'react';
import '../resources/css/Christmas.css';
import { connect } from 'react-redux';

class NavigationBar extends Component {
	static displayName = NavigationBar.name;

	constructor (props) {
		super(props);
		this.state = {
			collapsed: true
		};
		this.toggleNavbar = this.toggleNavbar.bind(this);
	}

	toggleNavbar () {
		this.setState({
			collapsed: !this.state.collapsed
		});
	}

	render() {
		let userIsValid = UserIsValid(this.props.auth);
		return (
			<header>
				<Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
					<Container>
						<img src={logo} alt="Holly" className="christmas-image" />
						<NavbarBrand tag={Link} to="/">Trevor's Secret Santa 2.0</NavbarBrand>
						<NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
						<Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
							<ul className="navbar-nav flex-grow">
								<NavItem>
									<NavLink exact className="text-dark nav-link" to='/'>Home</NavLink>
								</NavItem>
								<NavItem>
									<NavLink exact className="text-dark nav-link" to='/join-the-fun'>Join the Fun</NavLink>
								</NavItem>
								<NavItem>
									<NavLink exact className="text-dark nav-link" to={userIsValid ? '/logout' : '/login'}>{userIsValid ? 'Logout' : 'Login'}</NavLink>
								</NavItem>
							</ul>
						</Collapse>
					</Container>
				</Navbar>
			</header>
		);
	}
};

const mapStateToProps = (state) => {
	return {
        auth: state.auth,
        signalR: state.signalR
	};
};

export default withRouter(connect(mapStateToProps)(NavigationBar));