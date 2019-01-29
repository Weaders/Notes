import {
    ACTION_LOGIN_SUCCESS,
    ACTION_LOGIN_FAIL,
    ACTION_LOGIN_START,
    ACTION_REIGSTER_START,
    ACTION_REGISTER_FAIL,
    ACTION_LOGOUT,
    ACTION_GET_CURRENT_START,
    ACTION_GET_CURRENT_END
} from './../actions/user'

const initialState = {
    isLoading: false, userItem: null, errors: new Map()
};

export default (state = initialState, action) => {

    switch (action.type) {
        case ACTION_LOGIN_START:
        case ACTION_REIGSTER_START:
            return { ...state, isLoading: true };
        case ACTION_LOGIN_SUCCESS:
            return { userItem: action.userItem, errors: new Map(), isLoading: false };
        case ACTION_LOGIN_FAIL:
        case ACTION_REGISTER_FAIL:
            return { userItem: null, errors: action.reqError.errors, isLoading: false }
        case ACTION_LOGOUT:
            return { userItem: null, errors: new Map(), isLoading: false }
        case ACTION_GET_CURRENT_START:
            return { ...state, isLoading: true }
        case ACTION_GET_CURRENT_END:
            return { ...state, isLoading: false }
        default:
            return state;
    }

}