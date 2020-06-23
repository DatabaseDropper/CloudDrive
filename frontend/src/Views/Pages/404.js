import React from 'react';
import {Link} from 'react-router-dom';

import { Layout, Typography, Row, Col, Space } from 'antd';

const Page_404 = (props) => {
    return (
        <Layout>
            <Layout.Content>
                <Row align="middle" justify="center" style={{ minHeight: '100vh' }}>
                    <Col span={12} style={{textAlign: 'center'}}>
                        <Typography.Title>404</Typography.Title>
                        <Typography.Paragraph>
                            Strony nie znaleziono<br />
                            <Link to="/" component={Typography.Link}>Wróć do strony głównej</Link>
                        </Typography.Paragraph>
                    </Col>
                </Row>
            </Layout.Content>
        </Layout>
    );
}

export default Page_404;