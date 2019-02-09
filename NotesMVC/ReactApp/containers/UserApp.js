
import { withRouter } from 'react-router-dom';
import { withLocalize } from 'react-localize-redux';

import { connect } from 'react-redux';
import { getCurrent } from '../actions/user';
import App from '../components/App';

const mapStateToProps = state => ({
  isLoading: state.user.isLoading,
});

const mapDispatchToProps = (dispatch, ownProps) => Object.assign({
  onStart: () => {
    dispatch(getCurrent());
  },
}, ownProps);

export default withLocalize(withRouter(connect(mapStateToProps, mapDispatchToProps)(App)));
