import App from '../components/App'
import { getCurrent } from '../actions/user'
import { withRouter } from 'react-router-dom'

import { connect } from 'react-redux'

const mapStateToProps = function (state, ownProps) {

    return {
        isLoading: state.user.isLoading
    };

}

const mapDispatchToProps = (dispatch, ownProps) => {

    return Object.assign({
        onStart: () => {
            dispatch(getCurrent());
        }
    }, ownProps);

}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(App))