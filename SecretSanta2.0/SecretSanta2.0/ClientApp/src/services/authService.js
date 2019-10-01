import jwt from 'jsonwebtoken';
import Cookies from 'js-cookie';

export function UserIsValid(token) {
	let expiration;
	let now = new Date().getTime() / 1000;
	if (token != null && token && token.isAuthenticated) {
		expiration = jwt.decode(token.user).exp;
		if (expiration > now) {
			return true;
		}
	}
	let cookieToken = Cookies.get('Authorization-Token');
	if (cookieToken != null && cookieToken) {
		expiration = jwt.decode(cookieToken).exp;
		if (expiration > now) {
			return true;
		}
	}
	return false;
}

export function TryGetToken(token) {
	if (token != null && token) {
		return 'Bearer ' + token;
	}
	let cookieToken = Cookies.get('Authorization-Token');
	if (cookieToken != null && cookieToken) {
		return 'Bearer ' + cookieToken;
	}
	return '';
}
