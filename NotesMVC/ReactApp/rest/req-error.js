import { getTranslate } from 'react-localize-redux';
import _ from 'lodash';

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
      Object.keys(data.errors).forEach((key) => {
        this.errors.set(key, data.errors[key]);
      });
    }
  }

  /**
     * Get translated map of errors
     * @param {{}} localizeState
     * @returns {Map<string, string>}
     */
  getTranslatedErrors(localizeState) {
    const translatedMap = new Map();

    Array.from(this.errors).forEach(([errKey, errVal]) => {
      if (_.isString(errVal)) {
        translatedMap.set(errKey, getTranslate(localizeState)(errVal));
      } else {
        translatedMap.set(errKey, errVal);
      }
    });

    return translatedMap;
  }
}

export default ReqError;
