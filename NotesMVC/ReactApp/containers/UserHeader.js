import { connect } from 'react-redux';
import Header from '../components/Header';

const mapStateToProps = state => ({
  isLogoutShow: !!state.user.userItem,
});

export default connect(mapStateToProps)(Header);
