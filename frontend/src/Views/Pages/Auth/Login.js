import React from 'react';
import {Link} from 'react-router-dom';
import { Form, Input, Button, Typography } from 'antd';
import Layout_NoAuth from '../../Layouts/NoAuth';

class Page_Auth_Login extends React.Component {
    constructor(props) {
        super(props);

        this.state = {submitProcess: false, fields: {}, errors: []};

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        let newStateFields = this.state.fields;
        newStateFields[event.target.name] = (event.target.type === 'checkbox' ? event.target.checked : event.target.value);
        this.setState({ fields: newStateFields });
    }
    
    handleSubmit(event) {
    }

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