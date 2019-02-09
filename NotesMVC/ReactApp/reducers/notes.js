import _ from 'lodash';
import {
  ACTION_NOTES_GET_START,
  ACTION_NOTES_GET_FAIL,
  ACTION_NOTES_GET_SUCCESS,
  ACTION_NOTES_ADD_START,
  ACTION_NOTES_ADD_FAIL,
  ACTION_NOTES_ADD_SUCCESS,
  ACTION_NOTES_REMOVE_START,
  ACTION_NOTES_REMOVE_FAIL,
  ACTION_NOTES_REMOVE_SUCCESS,
  ACTION_NOTES_EDIT_START,
  ACTION_NOTES_EDIT_FAIL,
  ACTION_NOTES_EDIT_SUCCESS,
} from '../actions/notes';

const initialState = {
  notesArray: [],
  isLoadNotes: false,
  isLoadAddNote: false,
  reqErrorOnAdd: null,
  reqErrorOnEdit: new Map(),
  isRemoveLoadIds: [],
  isEditLoadIds: [],
};

export default (state = initialState, action) => {
  let newState = state;

  if (action.type === ACTION_NOTES_GET_START) {
    newState = {
      ...state,
      isLoadNotes: true,
    };
  } else if (action.type === ACTION_NOTES_GET_SUCCESS) {
    newState = {
      ...state,
      isLoadNotes: false,
      notesArray: action.notes,
    };
  } else if (action.type === ACTION_NOTES_GET_FAIL) {
    newState = {
      ...state,
      isLoadNotes: false,
    };
  } else if (action.type === ACTION_NOTES_ADD_START) {
    newState = {
      ...state,
      isLoadAddNote: true,
      reqErrorOnAdd: null,
    };
  } else if (action.type === ACTION_NOTES_ADD_FAIL) {
    newState = {
      ...state,
      isLoadAddNote: false,
      reqErrorOnAdd: action.reqError,
    };
  } else if (action.type === ACTION_NOTES_ADD_SUCCESS) {
    newState = {
      ...state,
      notesArray: [...state.notesArray, action.newItem],
    };
  } else if (action.type === ACTION_NOTES_REMOVE_START) {
    newState = {
      ...state,
      isRemoveLoadIds: [...state.isRemoveLoadIds, action.id],
    };
  } else if (action.type === ACTION_NOTES_REMOVE_FAIL) {
    newState = {
      ...state,
      isRemoveLoadIds: _.without(state.isRemoveLoadIds, action.id),
    };
  } else if (action.type === ACTION_NOTES_REMOVE_SUCCESS) {
    const newIds = [...state.isRemoveLoadIds];

    _.pull(newIds, action.id);

    const newItems = [...state.notesArray];

    _.remove(newItems, item => item.id === action.id);

    newState = {
      ...state,
      isRemoveLoadIds: newIds,
      notesArray: newItems,
    };
  } else if (action.type === ACTION_NOTES_EDIT_START) {
    newState = {
      ...state,
      isEditLoadIds: [...state.isEditLoadIds, action.id],
    };

    newState.reqErrorOnEdit.delete(action.id);

    return newState;
  } else if (action.type === ACTION_NOTES_EDIT_FAIL) {
    newState = {
      ...state,
      isEditLoadIds: _.without(state.isEditLoadIds, action.id),
    };

    newState.reqErrorOnEdit.set(action.id, action.reqError);

    return newState;
  } else if (action.type === ACTION_NOTES_EDIT_SUCCESS) {
    const noteIndex = _.findIndex(state.notesArray, ({ id }) => id === action.item.id);
    const newNotes = [...state.notesArray];

    newNotes[noteIndex] = action.item;

    newState = {
      ...state,
      notesArray: newNotes,
      isEditLoadIds: _.without(state.isEditLoadIds, action.item.id),
    };
  }

  return newState;
};
