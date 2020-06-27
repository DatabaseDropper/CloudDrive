import Config from './Config';

class ConfigFactory
{
    static create(type, conf) {
        return new Config(Object.assign({}, conf.global, conf[type]));
    }
}

export default ConfigFactory;