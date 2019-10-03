import registerServiceWorker from './registerServiceWorker';
import { BrowserRouter as Router } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.css';
import { Provider } from 'react-redux';
import ReactDOM from 'react-dom';
import store from './store';
import React from 'react';
import App from './App';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
	<Provider store={store}>
		<Router basename={baseUrl}>
			<App />
		</Router>
	</Provider>,
  rootElement);
registerServiceWorker();
