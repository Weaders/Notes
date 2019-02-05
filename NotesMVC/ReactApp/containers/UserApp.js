import App from '../components/App'
import { getCurrent } from '../actions/user'

import { withRouter } from 'react-router-dom'
import { withLocalize } from 'react-localize-redux'

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

export default withLocalize(withRouter(connect(mapStateToProps, mapDispatchToProps)(App)))