import Service_Storage from './Storage';

test('Service Storage - set / get / remove', () => {
    Service_Storage.set('test', 'test string');
    expect(Service_Storage.get('test')).toBe('test string');
    Service_Storage.remove('test');
    expect(Service_Storage.get('test')).toBeNull();
    

    Service_Storage.set('test-json', { test: 1 });
    expect(Service_Storage.get('test-json')).toEqual({ test: 1 });
    Service_Storage.remove('test-json');
    expect(Service_Storage.get('test-json')).toBeNull();
});

test('Service Storage - clear', () => {
    Service_Storage.set('test', 'test string');
    expect(Service_Storage.get('test')).toBe('test string');
    

    Service_Storage.set('test-json', { test: 1 });
    expect(Service_Storage.get('test-json')).toEqual({ test: 1 });

    Service_Storage.clear();

    expect(Service_Storage.get('test')).toBeNull();
    expect(Service_Storage.get('test-json')).toBeNull();
});