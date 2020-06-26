import React from 'react';
import {withRouter} from "react-router";

import { Link } from 'react-router-dom';
import { Form, Input, Button, Typography, Spin, Alert, Row, Col } from 'antd';

import Component_Text_Align from './../Components/Text/Align';

import Layout_Auth from './../Layouts/Auth';
import Component_Form_AbstractForm from './../Components/Form/AbstractForm';

class Page_FolderAdd extends Component_Form_AbstractForm {
    render() {
        return (
            <Layout_Auth>
                <Row>
                    <Col span={24}>
                        <Link to={("/folder/" + this.props.match.params.folderId)}></Link>
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
                            <Form.Item name="name" label="Nazwa folderu" required={false}>
                                <Input name="name" onChange={this.handleChange} value={this.state.fields.name} />
                            </Form.Item>
                            <Component_Text_Align.Center>
                                {this.state.submitProcess ?
                                    <Spin size="large" />
                                    :
                                    <Form.Item>
                                        <Button type="primary" htmlType="submit">
                                            Zaloguj się
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