import React from 'react';
import { connect } from 'react-redux';

import "antd/dist/antd.css";

import {
    BrowserRouter as Router,
    Switch,
    Route
} from 'react-router-dom';

import Page_Auth_Login from './Views/Pages/Auth/Login';
import Page_Auth_Register from './Views/Pages/Auth/Register';

import Page_Dashboard from './Views/Pages/Dashboard';
import Page_FileAdd from './Views/Pages/FileAdd';
import Page_Folder from './Views/Pages/Folder';
import Page_404 from './Views/Pages/404';

const App = ({state}) => {
    return (
        <Router>
            {(state.has('authData') && state.get('authData').token.length > 0) ?
                <Switch>
                    <Route exact path="/file/add/:folderId">
                        <Page_FileAdd />
                    </Route>
                    <Route exact path="/folder/add/:folderId">
                        <div>folder add</div>
                    </Route>
                    <Route exact path="/folder/:folderId">
                        <Page_Folder />
                    </Route>
                    <Route exact path="/">
                        <Page_Dashboard folderId={state.get('authData').diskInfo.folderId} />
                    </Route>
                    <Route path="*">
                        <Page_404/>
                    </Route>
                </Switch>
              :
                <Switch>
                    <Route path="/rejestracja">
                        <Page_Auth_Register />
                    </Route>
                    <Route exact path="/">
                        <Page_Auth_Login/>
                    </Route>
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