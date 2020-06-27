import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import store from './Store';
import { authSetAccountInfo } from './Actions';

import App from './App';
import * as serviceWorker from './serviceWorker';

import Service_Api from './Service/Api';

setInterval(() => {
    if (store.getState().has('authData')) {
        Service_Api.fetch('/api/v1/Account', {
            data: {
                method: 'GET',
            },
            onSuccess: (res) => {
                if (200 == res.status) {
                    res.json()
                    .then((res) => {
                        store.dispatch(authSetAccountInfo(res));
                    });
                }
            }
        });
    }
}, 5000);

ReactDOM.render(
    <Provider store={store}>
        <React.StrictMode>
            <App />
        </React.StrictMode>
    </Provider>,
    document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
