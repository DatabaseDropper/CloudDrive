import React from 'react';
import {Link} from 'react-router-dom';
import { Form, Input, Button, Typography } from 'antd';

import Component_Form_AbstractForm from './../../Components/Form/AbstractForm';
import Layout_NoAuth from '../../Layouts/NoAuth';

class Page_Auth_Login extends Component_Form_AbstractForm {
    render() {
        return (
            <Layout_NoAuth pageTitle="Logowanie">
                <Typography.Paragraph>
                    Nie masz konta: <Link to="/rejestracja">Zarejestruj się</Link>
                </Typography.Paragraph>
                <Form onFinish={this.handleSubmit} layout="vertical">
                    <Form.Item name="loginOrEmail" label="Login lub E-mail" required={true}>
                        <Input name="loginOrEmail" onChange={this.handleChange} value={this.state.fields.login} />
                    </Form.Item>
                    <Form.Item name="password" label="Hasło" required={true}>
                        <Input name="password" type="password" onChange={this.handleChange} value={this.state.fields.password} />
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" htmlType="submit">
                            Zaloguj się
                        </Button>
                    </Form.Item>
                </Form>
            </Layout_NoAuth>
        );
    };
}

export default Page_Auth_Login;