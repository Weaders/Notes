import queryString from 'query-string';
import ReqError from './req-error';

class RestClient {
  constructor(prefix = '/') {
    this.prefix = prefix;
  }

  /**
   * Send post request
   * @async
   * @param {string} endpoint
   * @param {{}} data
   * @returns {Promise<{}>|Promise<ReqError>}
   */
  async post(endpoint, data) {
    const fetchResponse = await fetch(`${this.prefix}${endpoint}`, {
      method: 'POST',
      headers: {
        'Content-type': 'application/json',
      },
      body: JSON.stringify(data),
    });

    return RestClient.response(fetchResponse);
  }

  /**
     * Send delete request
     * @param {string} endpoint
     */
  async delete(endpoint) {
    const fetchResponse = await fetch(`${this.prefix}${endpoint}`, {
      method: 'DELETE',
    });

    return RestClient.response(fetchResponse);
  }

  /**
     * Send get request
     * @param {string} endpoint
     * @param {{}} queryObj
     */
  async get(endpoint, queryObj) {
    const queryStr = queryString.stringify(queryObj);

    const fetchResponse = await fetch(`${this.prefix}${endpoint}?${queryStr}`, {
      headers: {
        'Content-type': 'application/json',
      },
    });

    return RestClient.response(fetchResponse);
  }

  /**
     * @param {Response} fetchResponse
     */
  static async response(fetchResponse) {
    if (fetchResponse.status === 200) {
      return fetchResponse.json();
    }

    let body = {};

    if (fetchResponse.status !== 401) {
      try {
        body = await fetchResponse.json();
      } catch {
        // console.warn('Get body, that can not be parsed like json');
      }
    }

    throw new ReqError(body, fetchResponse.status);
  }
}

export default RestClient;
