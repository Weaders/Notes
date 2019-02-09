export const ACTION_SET_CODE = 'SET_CODE';

export const setCode = newCode => (dispatch) => {
  dispatch({
    type: ACTION_SET_CODE,
    code: newCode,
  });
};
