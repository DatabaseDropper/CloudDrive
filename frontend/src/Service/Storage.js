class Service_Storage {

    set(key, value) {
        value = JSON.stringify(value);
        window.localStorage.setItem(key, value);
    }
    
    get(key) {
        let value = window.localStorage.getItem(key);
        
        try {
            return JSON.parse(value);
        } catch(e) {
            return null;
        }
    }
    
    remove(key) {
        window.localStorage.removeItem(key);
    }
    
    clear() {
        window.localStorage.clear();
    }
}

const inst = new Service_Storage();
export default inst;