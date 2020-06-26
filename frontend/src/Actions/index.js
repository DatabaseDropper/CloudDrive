export const authLogout = () => ({
    type: 'AUTH_LOGOUT'
});

export const authLogin = (authData) => ({
    type: 'AUTH_LOGIN',
    authData: authData
});

export const authSetAccountInfo = (authAccountInfo) => ({
    type: 'AUTH_SET_ACCOUNT_INFO',
    authAccountInfo: authAccountInfo
});