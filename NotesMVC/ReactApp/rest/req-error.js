class ReqError {
    
    /**
     * Init request error.
     * @param {{errors: {}}|{}} data 
     */
    constructor(data, code) {

        /**
         * Map of errors
         * @type {Map<string, string>}
         */
        this.errors = new Map();

        this.statusCode = code || null;

        if (data && data.errors) {

            for (let key in data.errors) {

                if (!Object.prototype.hasOwnProperty.call(data.errors, key)) {
                    continue;
                }

                this.errors.set(key, data.errors[key]);

            }

        }

    }

}

export default ReqError;