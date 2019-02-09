import {
  ACTION_LOGIN_SUCCESS,
  ACTION_LOGIN_FAIL,
  ACTION_LOGIN_START,
  ACTION_REIGSTER_START,
  ACTION_REGISTER_FAIL,
  ACTION_LOGOUT,
  ACTION_GET_CURRENT_START,
  ACTION_GET_CURRENT_END,
} from '../actions/user';

const initialState = {
  isLoading: false,
  userItem: null,
  reqError: null,
};

export default (state = initialState, action) => {
  switch (action.type) {
    case ACTION_LOGIN_START:
    case ACTION_REIGSTER_START:
      return {
        ...state,
        isLoading: true,
      };
    case ACTION_LOGIN_SUCCESS:
      return {
        ...state,
        userItem: action.userItem,
        reqError: null,
        isLoading: false,
      };
    case ACTION_LOGIN_FAIL:
    case ACTION_REGISTER_FAIL:

      return {
        ...state,
        userItem: null,
        reqError: action.reqError,
        isLoading: false,
      };

    case ACTION_LOGOUT:

      return {
        ...state,
        userItem: null,
        reqError: null,
        isLoading: false,
      };

    case ACTION_GET_CURRENT_START:

      return {
        ...state,
        isLoading: true,
      };

    case ACTION_GET_CURRENT_END:

      return {
        ...state,
        isLoading: false,
      };

    default:
      return state;
  }
};
