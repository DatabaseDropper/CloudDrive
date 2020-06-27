import React from 'react';

import Layout_Auth from './../Layouts/Auth';
import Components_Folder from '../Components/Folder';

const Page_Dashboard = (props) => {
    return (
        <Layout_Auth>
            <Components_Folder folderId={props.folderId}></Components_Folder>
        </Layout_Auth>
    );
}

export default Page_Dashboard;