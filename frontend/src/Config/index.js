import ConfigFactory from './ConfigFactory';

const config = {
    global: {},
    production: {
        apiUrl: ''
    },
    development: {
        apiUrl: 'https://localhost:5001'
    }
};

const inst = ConfigFactory.create(process.env.NODE_ENV, config);
export default inst;
