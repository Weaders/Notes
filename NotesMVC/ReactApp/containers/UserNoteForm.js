import { connect } from 'react-redux';
import NoteForm from '../components/NoteForm';
import { addNote, editNote } from '../actions/notes';

const mapStateToProps = (state, ownProps) => {
  const props = {
    secretCode: state.secretCode,
    errors: new Map(),
  };

  if (ownProps.id) {
    const reqError = state.notes.reqErrorOnEdit.get(ownProps.id);

    if (reqError) {
      props.errors = reqError.getTranslatedErrors(state.localize);
    }
  } else if (state.notes.reqErrorOnAdd) {
    props.errors = state.notes.reqErrorOnAdd.getTranslatedErrors(state.localize);
  }

  return props;
};

const mapDispatchToProps = (dispatch, ownProps) => Object.assign({
  /**
         * @param {NoteForm} form
         */
  onFormSend: (form) => {
    if (form.props.id) {
      dispatch(editNote(form.props.id, form.state.title, form.state.text, form.props.secretCode));
    } else {
      dispatch(addNote(form.state.title, form.state.text, form.props.secretCode));
    }
  },
}, ownProps);

export default connect(mapStateToProps, mapDispatchToProps)(NoteForm);
