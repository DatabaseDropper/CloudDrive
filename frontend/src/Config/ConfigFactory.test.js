import ConfigFactory from './ConfigFactory';

test('ConfigFactory - create config only by global config', () => {
    let config = ConfigFactory.create('global', {
        global: {
            test: 'test string'
        }
    });

    expect(config.get('test')).toBe('test string');
});

test('ConfigFactory - create concrete type of config', () => {
    let config = ConfigFactory.create('development', {
        global: {
            test: 'test string'
        },
        development: {
            test: 'test string development'
        }
    });

    expect(config.get('test')).toBe('test string development');
});