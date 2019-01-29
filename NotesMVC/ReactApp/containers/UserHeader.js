import NotesList from '../components/NotesList'
import { getNotes } from '../actions/notes'

import { connect } from 'react-redux'
import Header from './../components/Header'

const mapStateToProps = function (state, ownProps) {

    return {
        isLogoutShow: !!state.user.userItem
    };

}

export default connect(mapStateToProps)(Header)