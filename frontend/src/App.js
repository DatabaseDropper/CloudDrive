import React from 'react';
import { connect } from 'react-redux';

import "antd/dist/antd.css";

import {
  BrowserRouter as Router,
  Switch,
  Route
} from 'react-router-dom';

import Page_404 from './Views/Pages/404';

const App = ({state}) => {
  return (
    <Router>
        {false ? //TODO: sprawdzanie czy uzytkownik jest zalogowany po zrobieniu funkcjonalności logowania i rejestracji
              <Switch>
                  <Route path="*">
                    <Page_404/>
                  </Route>
              </Switch>
            :
              <Switch>
                  <Route path="*">
                      <Page_404/>
                  </Route>
              </Switch>
        }
    </Router>
  );
};

const mapStateToProps = state => ({
  state: state
});

export default connect(
  mapStateToProps
)(App);