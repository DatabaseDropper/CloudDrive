import React from 'react';
import {withRouter} from "react-router";

import { Link } from 'react-router-dom';
import { Form, Input, Button, Typography, Spin, Alert, Row, Col } from 'antd';
import { LeftOutlined } from '@ant-design/icons';

import Service_Api from './../../Service/Api';

import Component_Text_Align from './../Components/Text/Align';

import Layout_Auth from './../Layouts/Auth';
import Component_Form_AbstractForm from './../Components/Form/AbstractForm';

const defaultErrorMsg = 'Wystąpił błąd podczas dodawania folderu. Sprubuj jeszcze raz';

class Page_FolderAdd extends Component_Form_AbstractForm {
    handleSubmit() {
        this.setState({ submitProcess: true });

        Service_Api.fetch('/api/v1/Folder/' + this.props.match.params.folderId, {
            data: {
                method: 'POST',
                body: JSON.stringify(this.state.fields),
            },
            onSuccess: (res) => {
                if (200 == res.status) {
                    
                    res.json()
                    .then((res) => {
                        this.props.history.push('/folder/' + this.props.match.params.folderId);
                    });
                } else if(400 == res.status) {
                    res.json()
                    .then((res) => {
                        console.log(res);
                        let errors = [];

                        for(const key in res.errors) {
                            if (res.errors.hasOwnProperty(key)) {
                                errors.push(res.errors[key]);
                            }
                        }

                        this.setState({ submitProcess: false, errors: errors });
                    })
                    .catch(() => {
                        this.setState({ submitProcess: false, errors: [defaultErrorMsg] });
                    });
                } else {
                    this.setState({ submitProcess: false, errors: [defaultErrorMsg] });
                }
            },
            onError: (error) => {
                this.setState({ submitProcess: false, errors: [defaultErrorMsg] });
            }
        });
    }
    
    render() {
        return (
            <Layout_Auth>
                <Row gutter={[16, 16]}>
                    <Col span={24}>
                        <Link to={("/folder/" + this.props.match.params.folderId)}>
                            <LeftOutlined />
                            Wróć
                        </Link>
                    </Col>
                </Row>
                <Row>
                    <Col span={24}>
                        <Typography.Title>
                            Dodaj folder
                        </Typography.Title>
                        {this.state.errors.length > 0 ?
                            <div>
                            {this.state.errors.map((errorMsg, index) => {
                                return <div><Alert type="error" message={errorMsg} /><br/></div>
                            })}
                            </div>
                        : null }
                        <Form onFinish={this.handleSubmit} layout="vertical">
                            <Form.Item name="name" label="Nazwa folderu" required={true}>
                                <Input name="name" onChange={this.handleChange} value={this.state.fields.name} />
                            </Form.Item>
                            <Component_Text_Align.Center>
                                {this.state.submitProcess ?
                                    <Spin size="large" />
                                    :
                                    <Form.Item>
                                        <Button type="primary" htmlType="submit">
                                            Dodaj
                                        </Button>
                                    </Form.Item>
                                }
                            </Component_Text_Align.Center>
                        </Form>
                    </Col>
                </Row>
            </Layout_Auth>
        );
    }
}

export default withRouter(Page_FolderAdd);