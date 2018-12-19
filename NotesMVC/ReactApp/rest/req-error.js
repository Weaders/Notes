import _ from 'lodash'

class ReqError {
    
    /**
     * Init request error.
     * @param {{errors: {}}|string} data 
     */
    constructor(data) {

        if (_.isString(data)) {
            data = JSON.parse(data);
        }

        /**
         * Map of errors
         * @type {Map<string, string>}
         */
        this.errors = new Map();

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