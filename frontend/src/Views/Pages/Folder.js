import React from 'react';
import {withRouter} from "react-router";

import Components_AuthFolder from './../Components/AuthFolder';

const Page_Folder = (props) => {
    return (
        <Components_AuthFolder folderId={props.match.params.folderId} />
    );
}

export default withRouter(Page_Folder);