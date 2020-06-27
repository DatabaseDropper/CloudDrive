class Config
{
    constructor(config) {
        this.variables = config;
    }

    get(key) {
        return this.variables[key];
    }
}

export default Config;