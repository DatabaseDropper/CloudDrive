import Config from './Config';

test('Config', () => {
    let config = new Config({test: 'test string'});
    expect(config.get('test')).toBe('test string');
});