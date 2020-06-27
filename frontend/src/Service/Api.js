import Config from './../Config';
import { authLogout } from './../Actions'
import store from './../Store'

class Service_Api {
    getPermalink(url) {
        return Config.get('apiUrl') + url;
    }

    getAuthorizationHeaders() {
        let authHeaders = {};
        
        if (store.getState().has('authData')) {
            authHeaders = {
                'Authorization': 'Bearer ' + store.getState().get('authData').token
            };
        }

        return authHeaders;
    }

    parseHeaders(headers = {}) {
        return Object.assign({}, {
            'Content-Type': 'application/json;charset=utf-8',
        }, this.getAuthorizationHeaders(), headers);
    }

    fetch(url, settings) {
        url = this.getPermalink(url);

        settings.data.headers = this.parseHeaders(settings.data.headers);

        settings = Object.assign({
            data: {},
            onSuccess: (res) => {},
            onError: (error) => {}
        }, settings);

        return fetch(url, settings.data)
            .then((res) => {
                if (res.status == 401) {
                    store.dispatch(authLogout());
                } else {
                    settings.onSuccess(res);
                }
            })
            .catch(settings.onError);
    }
}

const inst = new Service_Api();
export default inst;