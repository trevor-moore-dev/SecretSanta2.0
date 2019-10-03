import { createStore, combineReducers, applyMiddleware } from 'redux';
import signalR from './reducers/signalRReducer';
import { createLogger } from 'redux-logger';
import auth from './reducers/authReducer';
import thunk from 'redux-thunk';

export default createStore(combineReducers({
    auth,
    signalR
}),
    {},
    applyMiddleware(createLogger(), thunk)
);
