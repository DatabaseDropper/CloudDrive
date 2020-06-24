export const authLogout = () => ({
    type: 'AUTH_LOGOUT'
});

export const authLogin = (authData) => ({
    type: 'AUTH_LOGIN',
    authData: authData
});