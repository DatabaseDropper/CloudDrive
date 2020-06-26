import React from 'react';
import {withRouter} from 'react-router';

import { Typography, List, Card, Button, Spin, Modal, Link } from 'antd';
import { ExclamationCircleOutlined } from '@ant-design/icons';

import Service_Api from '../../../../Service/Api';

const defaultDownloadFileErrorMsg = 'Wystąpił błąd podczas pobierania pliku. Sprubuj jeszcze raz';
const defaultDeleteFileErrorMsg = 'Wystąpił błąd podczas usuwania pliku. Sprubuj jeszcze raz';

class Component_List_Item_File extends React.Component {
    constructor(props) {
        super(props);

        this.state = { fileChangeProcess: false };

        this.handleDownload = this.handleDownload.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
    }

    handleDownload() {
        this.setState({ fileChangeProcess: true });

        fetch(Service_Api.getPermalink('/api/v1/File/' + this.props.file.id), {
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
            link.setAttribute('download', this.props.file.name);
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

                Service_Api.fetch('/api/v1/File/' + this.props.file.id, {
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
                <Card title={this.props.file.name}>
                    {this.state.fileChangeProcess ? 
                            <Spin />
                        :
                            <div>
                                <Typography.Text >
                                    <Button type="link" onClick={this.handleDownload}>pobierz</Button> 
                                </Typography.Text>
                                <Typography.Text type="danger">
                                    <Button type="text" danger onClick={this.handleDelete}>usuń</Button> 
                                </Typography.Text>
                            </div>
                    }
                </Card>
            </List.Item>
        );
    }
}

export default withRouter(Component_List_Item_File);