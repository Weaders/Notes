import { ACTION_SET_CODE } from '../actions/secret-code';

export default (state = '', action) => {
  switch (action.type) {
    case ACTION_SET_CODE:
      return action.code;
    default:
      return state;
  }
};
