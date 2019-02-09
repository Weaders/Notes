import RestClient from '../rest/rest-client';
import NoteItem from '../models/NoteItem';

export const ACTION_NOTES_GET_START = 'NOTES_GET_START';
export const ACTION_NOTES_GET_SUCCESS = 'NOTES_GET_SUCCESS';
export const ACTION_NOTES_GET_FAIL = 'NOTES_GET_FAIL';

export const ACTION_NOTES_ADD_START = 'NOTES_ADD_START';
export const ACTION_NOTES_ADD_FAIL = 'NOTES_ADD_FAIL';
export const ACTION_NOTES_ADD_SUCCESS = 'NOTES_ADD_SUCCESS';

export const ACTION_NOTES_EDIT_START = 'NOTES_EDIT_START';
export const ACTION_NOTES_EDIT_FAIL = 'NOTES_EDIT_FAIL';
export const ACTION_NOTES_EDIT_SUCCESS = 'NOTES_EDIT_SUCCESS';

export const ACTION_NOTES_REMOVE_START = 'NOTES_REMOVE_START';
export const ACTION_NOTES_REMOVE_FAIL = 'NOTES_REMOVE_FAIL';
export const ACTION_NOTES_REMOVE_SUCCESS = 'NOTES_REMOVE_SUCCESS';

const restClient = new RestClient();

export const getNotes = secretCode => async (dispatch) => {
  dispatch({
    type: ACTION_NOTES_GET_START,
  });

  let notes = [];

  try {
    const data = await restClient.get('notes/list', { key: secretCode });
    notes = data.map(n => new NoteItem(n.id, n.title, n.text));
  } catch (error) {
    dispatch({
      type: ACTION_NOTES_GET_FAIL,
      error,
    });

    return;
  }

  dispatch({
    type: ACTION_NOTES_GET_SUCCESS,
    notes,
  });
};

export const addNote = (title, text, secretCode) => (async (dispatch) => {
  dispatch({ type: ACTION_NOTES_ADD_START });

  let data = null;

  try {
    data = await restClient.post('notes/add', {
      text,
      title,
      secretKey: secretCode,
    });
  } catch (reqError) {
    dispatch({
      type: ACTION_NOTES_ADD_FAIL,
      reqError,
    });

    return;
  }

  const newItem = new NoteItem(data.id, data.title, data.text);

  dispatch({
    type: ACTION_NOTES_ADD_SUCCESS,
    newItem,
  });
});


export const editNote = (id, title, text, secretCode) => async (dispatch) => {
  let data = null;

  dispatch({ type: ACTION_NOTES_EDIT_START, id });

  try {
    data = await restClient.post(`notes/${id}/edit`, {
      text,
      title,
      id,
      secretKey: secretCode,
    });
  } catch (reqError) {
    dispatch({ type: ACTION_NOTES_EDIT_FAIL, reqError, id });
    return;
  }

  dispatch({
    type: ACTION_NOTES_EDIT_SUCCESS,
    item: new NoteItem(data.id, data.title, data.text),
  });
};

export const removeNote = id => async (dispatch) => {
  dispatch({ type: ACTION_NOTES_REMOVE_START, id });

  try {
    await restClient.delete(`notes/${id}`);
  } catch (reqError) {
    dispatch({
      type: ACTION_NOTES_REMOVE_FAIL,
      id,
    });
  }

  dispatch({
    type: ACTION_NOTES_REMOVE_SUCCESS,
    id,
  });
};
