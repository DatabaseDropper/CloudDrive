import React from 'react';
import {Link} from 'react-router-dom';

import { Layout, Typography, Row, Col } from 'antd';

const Page_404 = (props) => {
    return (
        <main style={{ padding: '0 20px' }}>
            <Row align="middle" justify="center" style={{ minHeight: '100vh' }}>
                <Col span={24} style={{textAlign: 'center'}}>
                    <Typography.Title>404</Typography.Title>
                    <Typography.Paragraph>
                        Strony nie znaleziono<br />
                        <Link to="/">Wróć do strony głównej</Link>
                    </Typography.Paragraph>
                </Col>
            </Row>
        </main>
    );
}

export default Page_404;