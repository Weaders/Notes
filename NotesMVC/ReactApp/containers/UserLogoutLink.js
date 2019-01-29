import LogoutLink from '../components/LogoutLink'
import { logout } from '../actions/user'

import { connect } from 'react-redux'

const mapStateToProps = function (state, ownProps) {
    return {};
}

const mapDispatchToProps = (dispatch, ownProps) => {

    return Object.assign({
        /**
         * @param {NoteForm} form
         */
        onClick: () => {
            dispatch(logout());
        }
    }, ownProps);

}

export default connect(mapStateToProps, mapDispatchToProps)(LogoutLink)