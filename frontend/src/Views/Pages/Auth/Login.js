import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from "react-router";
import { Link } from 'react-router-dom';
import { Form, Input, Button, Typography, Spin, Alert } from 'antd';

import Component_Form_AbstractForm from './../../Components/Form/AbstractForm';
import Layout_NoAuth from '../../Layouts/NoAuth';
import Component_Text_Align from './../../Components/Text/Align';

import Service_Api from './../../../Service/Api';

import { authLogin } from './../../../Actions';

const defaultErrorMsg = 'Wystąpił błąd podczas logowania. Spróbuj ponownie później';

class Page_Auth_Login extends Component_Form_AbstractForm {
    handleSubmit() {
        this.setState({ submitProcess: true });

        Service_Api.fetch('/api/v1/Account/SignIn', {
            data: {
                method: 'POST',
                body: JSON.stringify(this.state.fields),
            },
            onSuccess: (res) => {
                if (200 == res.status) {
                    res.json()
                    .then((res) => {
                        this.props.authLogin(res);
                        this.props.history.push('/');
                    });
                } else if(400 == res.status) {
                    res.json()
                    .then((res) => {
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
            <Layout_NoAuth pageTitle="Logowanie">
                <Typography.Paragraph>
                    Nie masz konta: <Link to="/rejestracja">Zarejestruj się</Link>
                </Typography.Paragraph>
                {this.state.errors.length > 0 ?
                    <div>
                    {this.state.errors.map((errorMsg, index) => {
                        return <div key={index}><Alert type="error" message={errorMsg} /><br/></div>
                    })}
                    </div>
                : null }
                <Form onFinish={this.handleSubmit} layout="vertical">
                    <Form.Item name="loginOrEmail" label="Login lub E-mail" required={true}>
                        <Input name="loginOrEmail" onChange={this.handleChange} value={this.state.fields.login} />
                    </Form.Item>
                    <Form.Item name="password" label="Hasło" required={true}>
                        <Input name="password" type="password" onChange={this.handleChange} value={this.state.fields.password} />
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
            </Layout_NoAuth>
        );
    };
}

const mapStateToProps = state => ({
    state: state
});

const mapDispatchToProps = dispatch => ({
    authLogin: bindActionCreators(authLogin, dispatch)
});
  
export default withRouter(connect(
    mapStateToProps,
    mapDispatchToProps
)(Page_Auth_Login));