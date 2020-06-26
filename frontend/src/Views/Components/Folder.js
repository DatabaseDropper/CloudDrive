import React from 'react';
import { Link } from 'react-router-dom';

import { Button, Typography, Spin, Space, Row, Col } from 'antd';
import { ReloadOutlined, FileAddOutlined, FolderAddOutlined } from '@ant-design/icons';

import Component_Text_Align from './Text/Align';

import Service_Api from './../../Service/Api';
import store from './../../Store';

import Component_List_Folder from './../Components/List/Folder';
import Component_List_File from './../Components/List/File';

const defaultErrorMsg = 'Wystąpił błąd podczas ładowania folderu';

class Components_Folder extends React.Component {
    constructor(props) {
        super(props);

        this.state = { loadingFolder: true, folderData: false, errors: [] };

        this.loadFolder = this.loadFolder.bind(this);
    }

    componentDidMount() {
        this.loadFolder();
    }

    componentDidUpdate(prevProps, prevState) {
        if (this.props.folderId !== prevProps.folderId) {
            this.loadFolder();
        }
    }

    loadFolder() {
        this.setState({ loadingFolder: true, folderData: false, errors: [] });

        Service_Api.fetch('/api/v1/Folder/' + this.props.folderId, {
            data: {
                method: 'GET'
            },
            onSuccess: (res) => {
                if (200 == res.status) {
                    res.json()
                    .then((res) => {
                        this.setState({ loadingFolder: false, folderData: res, errors: [] });
                    });
                } else if(400 == res.status) {
                    res.json()
                    .then((res) => {
                        this.setState({ loadingFolder: false, errors: res.message });
                    })
                    .catch(() => {
                        this.setState({ loadingFolder: false, errors: [defaultErrorMsg] });
                    });
                } else {
                    this.setState({ loadingFolder: false, errors: [defaultErrorMsg] });
                }
            },
            onError: (error) => {
                this.setState({ loadingFolder: false, errors: [defaultErrorMsg] });
            }
        });
    }

    render() {
        return (
            <div>
                {this.state.loadingFolder ?
                    <Typography.Paragraph>
                        <Component_Text_Align.Center>
                            <Spin size="large" />
                        </Component_Text_Align.Center>
                    </Typography.Paragraph>
                :
                    <div>
                        {this.state.errors.length > 0 ?
                            <Typography.Paragraph type="danger">
                                <Component_Text_Align.Center>
                                    <Space direction="vertical" size="middle">
                                        {this.state.errors.map((errorMsg, index) => {
                                            return <div>{errorMsg}</div>
                                        })}
                                        <div>
                                            <Button type="primary" shape="circle" icon={<ReloadOutlined />} size="large" onClick={this.loadFolder} />
                                        </div>
                                    </Space>
                                </Component_Text_Align.Center>
                            </Typography.Paragraph>
                        :
                            <div>
                                <Row justify="end">
                                    <Col>
                                        <Space size="middle">
                                            <Link to={"/file/add/" + this.props.folderId}><FileAddOutlined /> Dodaj plik</Link>
                                            <Link to={"/folder/add/" + this.props.folderId}><FolderAddOutlined /> Dodaj folder</Link>
                                        </Space>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col span={24}>
                                        <Typography.Title>
                                            {this.state.folderData.name}
                                        </Typography.Title>
                                        <Typography.Title level={2}>
                                            Foldery podrzędne
                                        </Typography.Title>
                                        {this.state.folderData.folders.length === 0 ?
                                            <div>brak elementów do wyświetlenia</div>
                                            :
                                            <Component_List_Folder folders={this.state.folderData.folders} onFolderChange={this.loadFolder} />
                                        }
                                        <Typography.Title level={2}>
                                            Pliki
                                        </Typography.Title>
                                        {this.state.folderData.files.length === 0 ?
                                            <div>brak elementów do wyświetlenia</div>
                                            :
                                            <Component_List_File files={this.state.folderData.files} onFileChange={this.loadFolder} />
                                        }
                                    </Col>
                                </Row>
                            </div>
                        }
                    </div>
                }
            </div>
        );
    }
}

export default Components_Folder;