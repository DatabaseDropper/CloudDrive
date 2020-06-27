import React from 'react';
import { Link } from 'react-router-dom';

import { List } from 'antd';

import Component_List_Item_File from './Item/File';

const Component_List_File = (props) => {
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
        dataSource={props.files}
        renderItem={file => (
            <Component_List_Item_File file={file} onFileChange={props.onFileChange} />
        )}
    />
};

export default Component_List_File;