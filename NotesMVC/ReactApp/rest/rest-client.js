import queryString from 'query-string'
import ReqError from './req-error'

class RestClient {

    /**
     * Send post request
     * @async
     * @param {string} endpoint 
     * @param {{}} data 
     * @returns {Promise<{}>|Promise<ReqError>}
     */
    async post(endpoint, data) {

        let fetchResponse = await fetch('/' + endpoint, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(data)
        })

        return this.response(fetchResponse);


    }

    /**
     * Send delete request
     * @param {string} endpoint 
     */
    async delete(endpoint) {

        let fetchResponse = await fetch('/' + endpoint, {
            method: 'DELETE'
        })

        return this.response(fetchResponse);

    }

    /**
     * Send get request
     * @param {string} endpoint
     * @param {{}} queryObj
     */
    async get(endpoint, queryObj) {

        var queryStr = queryString.stringify(queryObj);

        let fetchResponse = await fetch('/' + endpoint + '?' + queryStr, {
            headers: {
                'Content-type': 'application/json'
            }
        })

        return this.response(fetchResponse);

    }

    async response(fetchResponse) {

        if (fetchResponse.status === 200) {
            return fetchResponse.json();
        } else {

            let body = '';

            if (fetchResponse.bodyUsed) {
                body = fetchResponse.json();
            } else {
                body = 'error';
            }

            let err = new ReqError(body, fetchResponse.status);

            throw err;
        }

    }

}

export default RestClient;