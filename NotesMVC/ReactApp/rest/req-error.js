import _ from 'lodash'

class ReqError {
    
    /**
     * Init request error.
     * @param {{errors: {}}|string} data 
     */
    constructor(data, code) {

        if (_.isString(data) && data) {
            data = JSON.parse(data);
        } else {
            data = {};
        }

        /**
         * Map of errors
         * @type {Map<string, string>}
         */
        this.errors = new Map();

        this.statusCode = code || null;

        if (data.errors) {

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