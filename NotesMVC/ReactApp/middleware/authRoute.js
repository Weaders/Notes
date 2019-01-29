import { ACTION_LOGIN_SUCCESS, ACTION_LOGOUT, ACTION_GET_CURRENT_END } from './../actions/user'

import history from './../common/history-app'

export default function () {

    return next => action => {
        
        if (action.type === ACTION_LOGIN_SUCCESS && history.location.pathname !== '/notes') {
            history.push('/notes');
        } else if (action.type === ACTION_LOGOUT && history.location.pathname !== '/') {
            history.push('/');
        } else if (action.type === ACTION_GET_CURRENT_END && !action.result && history.location.pathname !== '/') {
            history.push('/');
        }

        return next(action);

    }
}