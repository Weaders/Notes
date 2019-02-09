import { connect } from 'react-redux';
import { getTranslate } from 'react-localize-redux';
import KeyForm from '../components/KeyForm';
import { setCode } from '../actions/secret-code';
import { getNotes } from '../actions/notes';

const mapStateToProps = state => ({
  translate: getTranslate(state.localize),
});

const mapDispatchToProps = (dispatch, ownProps) => Object.assign({
  onSubmit: (keyForm) => {
    dispatch(setCode(keyForm.state.key));
    dispatch(getNotes(keyForm.state.key));
  },
}, ownProps);

export default connect(mapStateToProps, mapDispatchToProps)(KeyForm);
