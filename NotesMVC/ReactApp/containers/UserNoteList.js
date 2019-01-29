import NotesList from '../components/NotesList'
import { getNotes } from '../actions/notes'

import { connect } from 'react-redux'

const mapStateToProps = function (state, ownProps) {

    return {
        secretCode: state.secretCode,
        notes: state.notes.notesArray,
        isLoading: state.notes.isLoadNotes
    };

}

const mapDispatchToProps = (dispatch, ownProps) => {

    return Object.assign({
        getNotes: () => {
            dispatch(getNotes());
        }
    }, ownProps);

}

export default connect(mapStateToProps, mapDispatchToProps)(NotesList)