import UserFrom from '../components/UserForm'
import { login, register } from '../actions/user'

import { connect } from 'react-redux'

const mapStateToProps = function(state, ownProps) {

    return {
        errors: state.user.errors || new Map(),
        isLoading: state.user.isLoading || false
    };

}

const mapDispatchToProps = (dispatch, ownProps) => {

    return Object.assign({
        onLoginClick: (userForm) => {
            dispatch(login(userForm.state.user, userForm.state.pwd));
        },
        onRegisterClick: (userForm) => {
            dispatch(register(userForm.state.user, userForm.state.pwd));
        }
    }, ownProps);

}

const AUserForm = connect(mapStateToProps, mapDispatchToProps)(UserFrom)

export default AUserForm
