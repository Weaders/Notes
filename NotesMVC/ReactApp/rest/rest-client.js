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

        let req = new XMLHttpRequest();

        req.open('POST', '/' + endpoint, true);

        req.setRequestHeader("Content-type", "application/json");

        req.send(JSON.stringify(data));

        return this.resultHandler(req);

    }

    /**
     * Send delete request
     * @param {string} endpoint 
     */
    async delete(endpoint) {

        let req = new XMLHttpRequest();

        req.open('DELETE', '/' + endpoint, true);

        req.send();

        return this.resultHandler(req);

    }

    /**
     * Send get request
     * @param {string} endpoint
     * @param {{}} queryObj
     */
    async get(endpoint, queryObj) {

        var queryStr = queryString.stringify(queryObj);

        var req = new XMLHttpRequest();

        req.open('GET', '/' + endpoint + '?' + queryStr, true);
        req.setRequestHeader('Content-type', 'application/json');
        req.send();

        return this.resultHandler(req);

    }

    async resultHandler(req) {

        return new Promise((res, rej) => {

            req.onreadystatechange = () => {

                if (req.readyState == XMLHttpRequest.DONE) {
                    if (req.status === 200) {

                        if(req.responseText) {
                            return res(JSON.parse(req.responseText));
                        } else {
                            return res({}); // Result with empty object, if there no body.
                        }

                    } else {
                        return rej(new ReqError(req.responseText, req.status));
                    }

                }

            }

        });

    }

}

export default RestClient;