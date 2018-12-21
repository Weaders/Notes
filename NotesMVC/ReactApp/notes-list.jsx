import RestClient from './rest/rest-client'
import NoteForm from './note-form.jsx'
import NoteLine from './note-line.jsx'
import NoteItem from './models/note-item'
import SecretKeyForm from './secret-key-form.jsx'

import appData from './models/app-data'

class NotesList extends React.Component {

    /**
     * Create note list
     * @param {{}} props 
     */
    constructor(props) {

        super(props);

        /**
         * @type {{notes: NoteItem[], expandAdd: Boolean}}
         */
        this.state = {
            notes: [],
            expandAdd: false
        };

        this.restClient = new RestClient();

        this.fillNotes();

        this.onNoteAdd = this.onNoteAdd.bind(this);
        this.onAddLineClick = this.onAddLineClick.bind(this);
        this.onRemoveNote = this.onRemoveNote.bind(this);
        this.onSubmitSecretKey = this.onSubmitSecretKey.bind(this);
        this._getAddForm = this._getAddForm.bind(this);

    }

    /**
     * Get notes from server.
     */
    async fillNotes() {

        let data = await this.restClient.get('notes/list', { key: appData.secretCode });

        this.setState({
            notes: data.map(n => new NoteItem(n.id, n.title, n.text))
        });

    }

    /**
     * On note add.
     * @param {NoteForm} noteForm 
     * @param {NoteItem} noteItem 
     */
    onNoteAdd(noteForm, noteItem) {

        let notes = this.state.notes;

        notes.push(noteItem);

        this.setState({ notes });

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
     * On submit secret key form.
     * Set secret key and after update notes.
     * @param {SecretKeyForm} secretKeyForm 
     */
    onSubmitSecretKey(secretKeyForm) {

        appData.secretCode = secretKeyForm.state.key;
        this.fillNotes();

    }

    /**
     * Toggle show form for add new note
     * @param {Event} event 
     */
    onAddLineClick(event) {

        this.setState({expandAdd: !this.state.expandAdd});
        event.preventDefault();

    }

    /**
     * Get form for add new note.
     */
    _getAddForm() {

        return <div className="card">
            <div className="card-header" onClick={this.onAddLineClick}>
                Add
            </div>
            <div className="card-body">
                <NoteForm onFormSend={this.onNoteAdd}/>
            </div>
        </div>

    }

    render() {

        let result = [];

        for (var note of this.state.notes) {
            result.push(<NoteLine onRemove={this.onRemoveNote} key={`${appData.secretCode}-${note.id}`} noteItem={note} />);
        }

        return (<div className="note-list">
            <SecretKeyForm onSubmit={this.onSubmitSecretKey} />
            {result}
            {this._getAddForm()}
        </div>);

    }

}

export default NotesList