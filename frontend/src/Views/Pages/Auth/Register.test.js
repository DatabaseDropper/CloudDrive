import React from 'react';
import { render } from '@testing-library/react';
import {
    BrowserRouter as Router,
} from 'react-router-dom';
import '../../../Tests/Mocks/matchMedia.js';

import Page_Auth_Register from './Register';

test('Page Register - check is elements exists', () => {
    const { container } = render(<Router><Page_Auth_Register /></Router>);

    expect(container.querySelector('[name="login"]')).toBeInTheDocument();
    expect(container.querySelector('[name="password"]')).toBeInTheDocument();
    expect(container.querySelector('[name="email"]')).toBeInTheDocument();
    expect(container.querySelector('[name="userName"]')).toBeInTheDocument();
});

test('Page Register - check is password element has correct type', () => {
    const { container } = render(<Router><Page_Auth_Register /></Router>);

    expect(container.querySelector('[name="password"]').getAttribute('type')).toBe('password');
});