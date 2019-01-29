import NoteLine from '../components/NoteLine'
import { removeNote } from '../actions/notes'
import { connect } from 'react-redux'
import _ from 'lodash'

const mapStateToProps = function (state, ownProps) {

    let isLoading = state.notes.isEditLoadIds.indexOf(ownProps.noteItem.id) !== -1 || state.notes.isRemoveLoadIds.indexOf(ownProps.noteItem.id) !== -1

    return {
        isEdit: false,
        isExpanded: false,
        isLoading
    };

}

const mapDispatchToProps = (dispatch, ownProps) => {

    return Object.assign({
        onRemove: () => {
            dispatch(removeNote(ownProps.noteItem.id));
        }
    }, ownProps);

}

export default connect(mapStateToProps, mapDispatchToProps)(NoteLine)