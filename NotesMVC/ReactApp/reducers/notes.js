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
    ACTION_NOTES_EDIT_SUCCESS
} from './../actions/notes'

import _ from 'lodash'

const initialState = {
    notesArray: [],
    isLoadNotes: false,
    isLoadAddNote: false,
    errorStates: [],
    isRemoveLoadIds: [],
    isEditLoadIds: []
};

export default (state = initialState, action) => {

    switch (action.type) {

        case ACTION_NOTES_GET_START:
            return {
                ...state,
                isLoadNotes: true,
                errors: []
            };
        case ACTION_NOTES_GET_SUCCESS:
            return {
                ...state,
                isLoadNotes: false,
                notesArray: action.notes,
                errors: []
            };
        case ACTION_NOTES_GET_FAIL:
            return {
                ...state,
                isLoadNotes: false,
                errors: action.errors
            };
        case ACTION_NOTES_ADD_START:
            return {
                ...state,
                isLoadAddNote: true,
            }
        case ACTION_NOTES_ADD_FAIL:
            return {
                ...state,
                isLoadAddNote: false
            }
        case ACTION_NOTES_ADD_SUCCESS:
            return {
                ...state,
                notesArray: [...state.notesArray, action.newItem]

            }
        case ACTION_NOTES_REMOVE_START:
            
            return {
                ...state,
                isRemoveLoadIds: [...state.isRemoveLoadIds, action.id]
            }

        case ACTION_NOTES_REMOVE_FAIL:

            return {
                ...state,
                isRemoveLoadIds: _.without(state.isRemoveLoadIds, action.id)
            };

        case ACTION_NOTES_REMOVE_SUCCESS:

            let newIds = [...state.isRemoveLoadIds];

            _.pull(newIds, action.id);

            let newItems = [...state.notesArray];

            _.remove(newItems, (item) => item.id === action.id);

            return {
                ...state,
                isRemoveLoadIds: newIds,
                notesArray: newItems
            };
        
        case ACTION_NOTES_EDIT_START:
            
            return {
                ...state,
                isEditLoadIds: [...state.isEditLoadIds, action.id]
            }

        case ACTION_NOTES_EDIT_FAIL:
            
            return {
                ...state,
                isEditLoadIds: _.without(state.isEditLoadIds, action.id)
            }
        
        case ACTION_NOTES_EDIT_SUCCESS:
            
            let noteIndex = _.findIndex(state.notesArray, ({id}) => id == action.item.id);
            let newNotes = [...state.notesArray];

            newNotes[noteIndex] = action.item;
            
            return {
                ...state,
                notesArray: newNotes,
                isEditLoadIds: _.without(state.isEditLoadIds, action.item.id)
            };

        default:
            return state;

    }

}