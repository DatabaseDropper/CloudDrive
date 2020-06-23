import Immutable from 'immutable';
import { createStore } from 'redux';
import Reducers from './../Reducers';

import initialState from './initialState';

const store = createStore(Reducers, Immutable.Map(initialState));
export default store;