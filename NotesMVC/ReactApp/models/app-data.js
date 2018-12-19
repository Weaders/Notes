import EventsEmitter from 'events'

const EVENT_CODE_CHANGED = 'secret-code-change';

class AppData extends EventsEmitter {

    constructor() {

        super();

        this.user = '';
        this._secretCode = '';
    }

    get secretCode() {
        return this._secretCode;
    }

    set secretCode(val) {

        this._secretCode = val;
        this.emit(EVENT_CODE_CHANGED, val);

    }

}


export { EVENT_CODE_CHANGED };
export default new AppData();