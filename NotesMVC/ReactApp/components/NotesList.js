import UserNoteForm from './../containers/UserNoteForm'
import UserNoteLine from './../containers/UserNoteLine'
import SecretKeyForm from '../containers/SecretKeyForm'
import LoadingBox from './LoadingBox'
import { Translate } from 'react-localize-redux'

import PropTypes from 'prop-types'

class NotesList extends React.Component {

    static get propTypes() {
        return {
            secretCode: PropTypes.string.isRequired,
            notes: PropTypes.array.isRequired,
            getNotes: PropTypes.func.isRequired,
            isLoading: PropTypes.bool.isRequired
        }

    }

    /**
     * Create note list
     * @param {{}} props 
     */
    constructor(props) {

        super(props);

        /**
         * @type {{expandAdd: Boolean}}
         */
        this.state = {
            expandAdd: false
        };

        this.onAddLineClick = this.onAddLineClick.bind(this);
        this.onRemoveNote = this.onRemoveNote.bind(this);
        this._getAddForm = this._getAddForm.bind(this);

    }

    componentDidMount() {
        this.props.getNotes();
    }

    /**
     * On remove note
     * @param {NoteLine} noteLine 
     */
    onRemoveNote(noteLine) {

        this.state.notes.splice(this.state.notes.indexOf(noteLine.state.noteItem), 1);
        this.forceUpdate();

    }

    /**
     * Toggle show form for add new note
     * @param {Event} event 
     */
    onAddLineClick(event) {

        this.setState({ expandAdd: !this.state.expandAdd });
        event.preventDefault();

    }

    /**
     * Get form for add new note.
     */
    _getAddForm() {

        return <div className="card">
            <div className="card-header" onClick={this.onAddLineClick}>
                <Translate id="add" />
            </div>
            <div className="card-body">
                <UserNoteForm />
            </div>
        </div>

    }

    render() {

        let result = [];

        for (let note of this.props.notes) {
            result.push(<UserNoteLine key={`${note.id}`} noteItem={note} />);
        }

        return (<div className="note-list">
            {!this.props.isLoading || <LoadingBox />}
            <SecretKeyForm />
            {result}
            {this._getAddForm()}
        </div>);

    }

}

export default NotesList