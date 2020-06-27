import Service_Storage from './../Service/Storage';

export default (state, action) => {
    switch (action.type) {
        case 'AUTH_LOGIN':
            state = state.set('authData', action.authData);
            break;
        case 'AUTH_LOGOUT':
            state = state.clear();
            break;
        case 'AUTH_SET_ACCOUNT_INFO':
            state = state.set('authAccountInfo', action.authAccountInfo);
            break;
        default:
            return state;
    }

    Service_Storage.set('cloud_drive', state.toObject());
    return state;
}