import React from 'react';
import { Link } from 'react-router-dom';

import { List } from 'antd';

import Component_List_Item_Folder from './Item/Folder';

const Component_List_Folder = (props) => {
    return <List
        grid={{
            gutter: 16,
            xs: 1,
            sm: 2,
            md: 3,
            lg: 3,
            xl: 4,
            xxl: 4
        }}
        dataSource={props.folders}
        renderItem={folder => (
            <Component_List_Item_Folder folder={folder} onFolderChange={props.onFolderChange} />
        )}
    />
};

export default Component_List_Folder;