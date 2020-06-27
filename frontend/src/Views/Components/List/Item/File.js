import React from 'react';
import {withRouter} from 'react-router';

import { Typography, List, Card, Button, Spin, Modal, Tooltip, Space } from 'antd';
import { ExclamationCircleOutlined, DownloadOutlined, DeleteOutlined, EyeOutlined, EyeInvisibleOutlined, LinkOutlined } from '@ant-design/icons';

import Service_Api from '../../../../Service/Api';

const defaultShareFileErrorMsg = 'Wystąpił błąd podczas zmiany widoczności pliku. Sprubuj jeszcze raz';
const defaultDownloadFileErrorMsg = 'Wystąpił błąd podczas pobierania pliku. Sprubuj jeszcze raz';
const defaultDeleteFileErrorMsg = 'Wystąpił błąd podczas usuwania pliku. Sprubuj jeszcze raz';

class Component_List_Item_File extends React.Component {
    constructor(props) {
        super(props);

        this.state = { fileChangeProcess: false, file: props.file };

        this.handleShare = this.handleShare.bind(this);
        this.handleGetShareLink = this.handleGetShareLink.bind(this);
        this.handleDownload = this.handleDownload.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
    }

    handleShare() {
        this.setState({ fileChangeProcess: true });

        Service_Api.fetch('/api/v1/File/Share/' + this.state.file.id, {
            data: {
                method: 'POST',
            },
            onSuccess: (res) => {
                if (200 == res.status) {
                    res.json()
                    .then((res) => {
                        let file = this.state.file;
                        file.isPublic = res.isPublic;
                        this.setState({ fileChangeProcess: false, file: file });
                    })
                    .catch(() => {
                        Modal.error({
                            title: defaultShareFileErrorMsg,
                        });
    
                        this.setState({ fileChangeProcess: false });
                    });
                } else {
                    Modal.error({
                        title: defaultShareFileErrorMsg,
                    });

                    this.setState({ fileChangeProcess: false });
                }
            },
            onError: (error) => {
                Modal.error({
                    title: defaultShareFileErrorMsg,
                });

                this.setState({ fileChangeProcess: false });
            }
        });
    }

    handleGetShareLink() {
        Modal.info({
            title: 'Link do pliku',
            icon: <LinkOutlined />,
            content: (
                <Typography.Text>
                    {Service_Api.getPermalink('/api/v1/File/' + this.state.file.id)}
                </Typography.Text>
            )
        });
    }

    handleDownload() {
        this.setState({ fileChangeProcess: true });

        fetch(Service_Api.getPermalink('/api/v1/File/' + this.state.file.id), {
            method: 'GET',
            headers: Service_Api.getAuthorizationHeaders()
        })
        .then((res) => {
            if (200 == res.status) {
                return res.blob();
            } else {
                Modal.error({
                    title: defaultDownloadFileErrorMsg,
                });

                this.setState({ fileChangeProcess: false });
            }
        })
        .then((file) => {
            let objectUrl = window.URL.createObjectURL(file);

            let link = document.createElement('a');
            link.href = objectUrl;
            link.setAttribute('download', this.state.file.name);
            link.click();

            window.URL.revokeObjectURL(objectUrl);

            this.setState({ fileChangeProcess: false });
        })
        .catch(() => {
            Modal.error({
                title: defaultDownloadFileErrorMsg,
            });

            this.setState({ fileChangeProcess: false });
        });
    }

    handleDelete() {
        Modal.confirm({
            title: 'Jesteś pewien, że chcesz usunąć ten plik?',
            okText: 'Tak',
            cancelText: 'Nie',
            icon: <ExclamationCircleOutlined />,
            okType: 'danger',
            onOk: () => {
                this.setState({ fileChangeProcess: true });

                Service_Api.fetch('/api/v1/File/' + this.state.file.id, {
                    data: {
                        method: 'DELETE',
                    },
                    onSuccess: (res) => {
                        if (200 == res.status) {
                            this.props.onFileChange();
                        } else {
                            Modal.error({
                                title: defaultDeleteFileErrorMsg,
                            });

                            this.setState({ fileChangeProcess: false });
                        }
                    },
                    onError: (error) => {
                        Modal.error({
                            title: defaultDeleteFileErrorMsg,
                        });

                        this.setState({ fileChangeProcess: false });
                    }
                });
            }
          });
    }

    render() {
        return (
            <List.Item>
                <Card title={this.state.file.name}>
                    {this.state.fileChangeProcess ? 
                            <Spin />
                        :
                            <Space>
                                {this.state.file.isPublic ? 
                                        <Tooltip title="Pobierz link do pliku">
                                            <Button type="primary" shape="circle" icon={<LinkOutlined />} size="large" onClick={this.handleGetShareLink} />
                                        </Tooltip>
                                    :
                                        null
                                }
                                <Tooltip title={this.state.file.isPublic ? "Przestań udostępniać" : "Udostepnij"}>
                                    <Button type="primary" shape="circle" icon={this.state.file.isPublic ?  <EyeInvisibleOutlined /> : <EyeOutlined />} size="large" onClick={this.handleShare} />
                                </Tooltip>
                                <Tooltip title="pobierz">
                                    <Button type="primary" shape="circle" icon={<DownloadOutlined />} size="large" onClick={this.handleDownload} />
                                </Tooltip>
                                <Tooltip title="usuń">
                                    <Button type="danger" shape="circle" icon={<DeleteOutlined />} size="large" onClick={this.handleDelete} />
                                </Tooltip>
                            </Space>
                    }
                </Card>
            </List.Item>
        );
    }
}

export default withRouter(Component_List_Item_File);