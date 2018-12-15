import RestClient from './rest-client'
import NoteForm from './note-form.jsx'
import NoteLine from './note-line.jsx'
import NoteItem from './note-item'

import appData from './app-data'

class NotesList extends React.Component {

    /**
     * Init note list
     * @param {{secretKey: string}} props 
     */
    constructor(props) {

        super(props);

        /**
         * @type {{notes: NoteItem[], key: string, expandAdd: Boolean}}
         */
        this.state = {
            notes: [],
            secretKey: props.sercretKey,
            expandAdd: false
        };

        this.restClient = new RestClient();

        this.fillNotes();

        this.onNoteAdd = this.onNoteAdd.bind(this);
        this.onAddLineClick = this.onAddLineClick.bind(this);
        this.onRemoveNote = this.onRemoveNote.bind(this);

    }

    async fillNotes() {

        let data = await this.restClient.get('notes/list', { key: this.state.secretKey });

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

    onAddLineClick(event) {

        this.setState({expandAdd: !this.state.expandAdd});
        event.preventDefault();

    }

    _getForm() {

        return <div className="card">
            <div className="card-header" onClick={this.onAddLineClick}>
                Add
            </div>
            <div className="card-body">
                <NoteForm secretKey={this.state.secretKey} onFormSend={this.onNoteAdd}/>
            </div>
        </div>

    }

    render() {

        let result = [];

        for (var note of this.state.notes) {
            result.push(<NoteLine onRemove={this.onRemoveNote} key={note.id} secretKey={this.state.secretKey} noteItem={note} />);
        }

        return (<div className="note-list">
            {result}
            {this._getForm()}
        </div>);

    }

}

export default NotesList