import Immutable from 'immutable';
import { createStore } from 'redux';
import Reducers from './../Reducers';

const store = createStore(Reducers, Immutable.Map({}));
export default store;