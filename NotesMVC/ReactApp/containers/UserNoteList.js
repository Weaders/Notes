import { connect } from 'react-redux';
import NotesList from '../components/NotesList';
import { getNotes } from '../actions/notes';

const mapStateToProps = state => ({
  secretCode: state.secretCode,
  notes: state.notes.notesArray || [],
  isLoading: state.notes.isLoadNotes,
});

const mapDispatchToProps = (dispatch, ownProps) => Object.assign({
  getNotes: () => {
    dispatch(getNotes());
  },
}, ownProps);

export default connect(mapStateToProps, mapDispatchToProps)(NotesList);
