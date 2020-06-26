import React from 'react';

import { Link } from 'react-router-dom';
import { Row, Col, Space } from 'antd';
import { FileAddOutlined, FolderAddOutlined } from '@ant-design/icons';

import Layout_Auth from '../Layouts/Auth';
import Components_Folder from './Folder';

const Components_AuthFolder = (props) => {
    return (
        <Layout_Auth>
            <Row justify="end">
                <Col>
                    <Space size="middle">
                        <Link to={"/file/add/" + props.folderId}><FileAddOutlined /> Dodaj plik</Link>
                        <Link to={"/folder/add/" + props.folderId}><FolderAddOutlined /> Dodaj folder</Link>
                    </Space>
                </Col>
            </Row>
            <Row>
                <Col>
                    <Components_Folder folderId={props.folderId}></Components_Folder>
                </Col>
            </Row>
        </Layout_Auth>
    );
}

export default Components_AuthFolder;