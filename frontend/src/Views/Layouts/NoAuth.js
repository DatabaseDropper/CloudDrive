import React from 'react';

import { Layout, Typography, Row, Col, Input } from 'antd';

const Layout_NoAuth = (props) => {
    return (
        <main style={{ padding: '0 20px' }}>
            <Row align="middle" justify="center" style={{ minHeight: '100vh' }}>
                <Col span={24} md={12}>
                    <div style={{textAlign: 'center'}}>
                        <Typography.Title>
                            <img src="/logo.png" alt="Cloud Drive" />
                        </Typography.Title>
                        <Typography.Title level={2}>
                            {props.pageTitle}
                        </Typography.Title>
                    </div>
                    {props.children}
                </Col>
            </Row>
        </main>
    );
}

export default Layout_NoAuth;