import React from 'react';
import { render } from '@testing-library/react';
import {
    BrowserRouter as Router,
} from 'react-router-dom';
import '../../../Tests/Mocks/matchMedia.js';

import Page_Auth_Login from './Login';

test('Page Login - check is elements exists', () => {
    const { container } = render(<Router><Page_Auth_Login /></Router>);

    expect(container.querySelector('[name="loginOrEmail"]')).toBeInTheDocument();
    expect(container.querySelector('[name="password"]')).toBeInTheDocument();
});

test('Page Login - check is password element has correct type', () => {
    const { container } = render(<Router><Page_Auth_Login /></Router>);

    expect(container.querySelector('[name="password"]').getAttribute('type')).toBe('password');
});