import NoteForm from '../components/NoteForm'
import { addNote, editNote } from '../actions/notes'

import { connect } from 'react-redux'

const mapStateToProps = function (state, ownProps) {

    let props = {
        secretCode: state.secretCode
    };

    if (ownProps.id) {
        props.errors = state.notes.errorsOnEdit.get(ownProps.id) || new Map()
    } else {
        props.errors = state.notes.errorsOnAdd;
    }

    return props;

}

const mapDispatchToProps = (dispatch, ownProps) => {

    return Object.assign({
        /**
         * @param {NoteForm} form
         */
        onFormSend: (form) => {
            if (form.props.id) {
                dispatch(editNote(form.props.id, form.state.title, form.state.text, form.props.secretCode));
            } else {
                dispatch(addNote(form.state.title, form.state.text, form.props.secretCode));
            }
            
            
        }
    }, ownProps);

}

export default connect(mapStateToProps, mapDispatchToProps)(NoteForm)