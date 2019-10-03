import Cookies from 'js-cookie';
import jwt from 'jsonwebtoken';

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

export function login(token) {
    return dispath => {
        dispath({
            type: "LOGIN",
            payload: token
        });
    }
}

export function logout() {
    return dispath => {
        dispath({
            type: "LOGOUT",
            payload: ""
        });
    };
}