import { connect } from 'react-redux';
import NoteLine from '../components/NoteLine';
import { removeNote } from '../actions/notes';

const mapStateToProps = (state, ownProps) => {
  const isLoading = state.notes.isEditLoadIds.indexOf(ownProps.noteItem.id) !== -1
    || state.notes.isRemoveLoadIds.indexOf(ownProps.noteItem.id) !== -1;

  return {
    isEdit: false,
    isExpanded: false,
    isLoading,
  };
};

const mapDispatchToProps = (dispatch, ownProps) => Object.assign({
  onRemove: () => {
    dispatch(removeNote(ownProps.noteItem.id));
  },
}, ownProps);

export default connect(mapStateToProps, mapDispatchToProps)(NoteLine);
