import { connect } from 'react-redux';
import UserFrom from '../components/UserForm';
import { login, register } from '../actions/user';

const mapStateToProps = (state) => {
  let errors = new Map();

  if (state.user.reqError) {
    errors = state.user.reqError.getTranslatedErrors(state.localize);
  }

  return {
    errors,
    isLoading: state.user.isLoading || false,
  };
};

const mapDispatchToProps = (dispatch, ownProps) => Object.assign({
  onLoginClick: (user, pwd) => {
    dispatch(login(user, pwd));
  },
  onRegisterClick: (user, pwd) => {
    dispatch(register(user, pwd));
  },
}, ownProps);

const AUserForm = connect(mapStateToProps, mapDispatchToProps)(UserFrom);

export default AUserForm;
