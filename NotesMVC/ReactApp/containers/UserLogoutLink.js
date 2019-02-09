import { connect } from 'react-redux';
import LogoutLink from '../components/LogoutLink';
import { logout } from '../actions/user';

const mapDispatchToProps = (dispatch, ownProps) => Object.assign({
  /**
   * @param {NoteForm} form
   */
  onClick: () => {
    dispatch(logout());
  },
}, ownProps);

export default connect(() => ({}), mapDispatchToProps)(LogoutLink);
