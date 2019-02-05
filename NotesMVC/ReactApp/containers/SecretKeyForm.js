import KeyForm from '../components/KeyForm'
import { setCode } from '../actions/secret-code'

import { connect } from 'react-redux'
import { getNotes } from '../actions/notes';

import { getTranslate } from 'react-localize-redux'

const mapStateToProps = function(state, ownProps) {
    return {
        translate: getTranslate(state.localize)
    };
}

const mapDispatchToProps = (dispatch, ownProps) => {

    return Object.assign({
        onSubmit: (keyForm) => {
            
            dispatch(setCode(keyForm.state.key));
            dispatch(getNotes(keyForm.state.key));
            
        }
    }, ownProps);

}

export default connect(mapStateToProps, mapDispatchToProps)(KeyForm)