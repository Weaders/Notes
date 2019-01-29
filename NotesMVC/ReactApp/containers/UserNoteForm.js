import NoteForm from '../components/NoteForm'
import { addNote, editNote } from '../actions/notes'

import { connect } from 'react-redux'

const mapStateToProps = function (state, ownProps) {

    return {
        secretCode: state.secretCode
    };

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