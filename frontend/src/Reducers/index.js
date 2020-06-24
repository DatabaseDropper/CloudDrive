import { combineReducers } from 'redux';
import Immutable from 'immutable';

const authInitialState = Immutable.Map({ user: { token: '' }});

const auth = (state = authInitialState, action) => {
    switch (action.type) {
        case 'AUTH_LOGOUT':
            return state.set('user', { token: '' });
        default:
            return state;
    }
}

export default combineReducers({auth});