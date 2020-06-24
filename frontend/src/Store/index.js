import Immutable from 'immutable';
import { createStore } from 'redux';
import Reducers from './../Reducers';
import Service_Storage from './../Service/Storage';

const store = createStore(Reducers, Immutable.Map(Object.assign({}, Service_Storage.get('cloud_drive'))));
export default store;