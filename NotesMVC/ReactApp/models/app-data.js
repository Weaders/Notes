import EventsEmitter from 'events'
import Cookies from 'universal-cookie';
import UserItem from './user-item';

import RestClient from './../rest/rest-client'
import history from './../common/history-app';

const EVENT_CODE_CHANGED = 'event-secret-code-change';
const EVENT_USER_LOGOUT = 'event-user-logout';
const COOKIE_SECRET_CODE = 'secret-code-cookie';

class AppData extends EventsEmitter {

    constructor() {

        super();

        this._cookies = new Cookies();
        this._restClient = new RestClient();

        /**
         * @type {null|UserItem}
         */
        this.user = null;
        this._secretCode = this._secretCodeCookie;


    }

    /**
     * Send request for logout, and emit event EVENT_USER_LOGOUT
     */
    async logout() {

        try {

            await this._restClient.post('user/logout');
            history.push('/');

        } catch(reqError) {
            console.warn(reqError);
            return;
        }

        this.emit(EVENT_USER_LOGOUT);

    }

    get secretCode() {
        return this._secretCode;
    }

    set secretCode(val) {

        this._secretCode = val;
        this._secretCodeCookie = val; // Set cookie! =)

        this.emit(EVENT_CODE_CHANGED, val);

    }

    get _secretCodeCookie() {
        return this._cookies.get(COOKIE_SECRET_CODE) || '';
    }

    set _secretCodeCookie(val) {

        let expires = new Date();

        expires.setHours(expires.getHours() + 1);

        this._cookies.set(COOKIE_SECRET_CODE, val, { expires });

    }



}


export { EVENT_CODE_CHANGED, COOKIE_SECRET_CODE };
export default new AppData();