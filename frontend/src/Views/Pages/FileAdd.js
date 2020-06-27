import React from 'react';
import {withRouter} from "react-router";

import { Link } from 'react-router-dom';
import { Upload, Typography, Spin, Alert, Row, Col } from 'antd';
import { LeftOutlined, InboxOutlined } from '@ant-design/icons';

import Service_Api from '../../Service/Api';

import Layout_Auth from '../Layouts/Auth';

class Page_FileAdd extends React.Component {
    render() {
        return (
            <Layout_Auth>
                <Row gutter={[16, 16]}>
                    <Col span={24}>
                        <Link to={("/folder/" + this.props.match.params.folderId)}>
                            <LeftOutlined />
                            Wróć do folderu
                        </Link>
                    </Col>
                </Row>
                <Row>
                    <Col span={24}>
                        <Typography.Title>
                            Dodaj plik
                        </Typography.Title>
                        <Upload.Dragger name="file" multiple={true} action={(Service_Api.getPermalink('/api/v1/File/' + this.props.match.params.folderId))} headers={Service_Api.getAuthorizationHeaders()}>
                            <p className="ant-upload-drag-icon">
                                <InboxOutlined />
                            </p>
                            <p className="ant-upload-text">
                                Kliknij lub przeciągnij plik do tego obszaru, aby go przesłać
                            </p>
                            <p className="ant-upload-hint">
                                Jednorazowo moesz tytaj przeciągnąć tylko jeden plik
                            </p>
                        </Upload.Dragger>
                    </Col>
                </Row>
            </Layout_Auth>
        );
    }
}

export default withRouter(Page_FileAdd);