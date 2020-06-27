import React from 'react';
import {withRouter} from "react-router";

import Layout_Auth from './../Layouts/Auth';
import Components_Folder from '../Components/Folder';

const Page_Folder = (props) => {
    return (
        <Layout_Auth>
            <Components_Folder folderId={props.match.params.folderId}></Components_Folder>
        </Layout_Auth>
    );
}

export default withRouter(Page_Folder);