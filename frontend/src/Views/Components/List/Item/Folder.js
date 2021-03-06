import React from 'react';
import {withRouter} from 'react-router';
import { Link } from 'react-router-dom';

import { Typography, List, Card, Button, Spin, Modal } from 'antd';
import { ExclamationCircleOutlined } from '@ant-design/icons';

import Service_Api from './../../../../Service/Api';

const defaultDeleteFolderErrorMsg = 'Wystąpił błąd podczas usuwania folderu. Sprubuj jeszcze raz';

class Component_List_Item_Folder extends React.Component {
    constructor(props) {
        super(props);

        this.state = { folderChangeProcess: false };

        this.handleDelete = this.handleDelete.bind(this);
    }

    handleDelete() {
        Modal.confirm({
            title: 'Jesteś pewien, że chcesz usunąć ten folder?',
            okText: 'Tak',
            cancelText: 'Nie',
            icon: <ExclamationCircleOutlined />,
            okType: 'danger',
            onOk: () => {
                this.setState({ folderChangeProcess: true });

                Service_Api.fetch('/api/v1/Folder/' + this.props.folder.id, {
                    data: {
                        method: 'DELETE',
                    },
                    onSuccess: (res) => {
                        if (200 == res.status) {
                            this.props.onFolderChange();
                        } else {
                            Modal.error({
                                title: defaultDeleteFolderErrorMsg,
                            });

                            this.setState({ folderChangeProcess: false });
                        }
                    },
                    onError: (error) => {
                        Modal.error({
                            title: defaultDeleteFolderErrorMsg,
                        });

                        this.setState({ folderChangeProcess: false });
                    }
                });
            }
          });
    }

    render() {
        return (
            <List.Item>
                <Card title={this.props.folder.name}>
                    {this.state.folderChangeProcess ? 
                            <Spin />
                        :
                            <div>
                                <Typography.Text >
                                    <Link to={('/folder/' + this.props.folder.id)}>zobacz</Link>
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

export default withRouter(Component_List_Item_Folder);