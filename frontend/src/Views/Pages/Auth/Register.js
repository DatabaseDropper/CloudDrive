import React from 'react';
import {Link} from 'react-router-dom';
import { Form, Input, Button, Typography } from 'antd';

import Component_Form_AbstractForm from './../../Components/Form/AbstractForm';
import Layout_NoAuth from '../../Layouts/NoAuth';

class Page_Auth_Register extends Component_Form_AbstractForm {
    render() {
        return (
            <Layout_NoAuth pageTitle="Rejestracja">
                <Typography.Paragraph>
                    Masz już konto: <Link to="/">Zaloguj się</Link>
                </Typography.Paragraph>
                <Form onFinish={this.handleSubmit} layout="vertical">
                    <Form.Item name="login" label="Login" required={true}>
                        <Input name="login" onChange={this.handleChange} value={this.state.fields.login} />
                    </Form.Item>
                    <Form.Item name="password" label="Hasło" required={true}>
                        <Input name="password" type="password" onChange={this.handleChange} value={this.state.fields.password} />
                    </Form.Item>
                    <Form.Item name="email" label="E-mail" required={true}>
                        <Input name="email" type="email" onChange={this.handleChange} value={this.state.fields.email} />
                    </Form.Item>
                    <Form.Item name="userName" label="Nazwa uytkownika" required={true}>
                        <Input name="userName" onChange={this.handleChange} value={this.state.fields.userName} />
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" htmlType="submit">
                            Zarejestruj się
                        </Button>
                    </Form.Item>
                </Form>
            </Layout_NoAuth>
        );
    };
}

export default Page_Auth_Register;