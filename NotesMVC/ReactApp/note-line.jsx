import NoteForm from "./note-form.jsx";
import RestClient from './rest-client'

class NoteLine extends React.Component {

    constructor(props) {

        super(props);

        /**
         * @type {{noteItem: NoteItem, isExapanded: Boolean, isEdit: Boolean, secretKey: string}}
         */
        this.state = {
            noteItem: props.noteItem,
            isExapanded: false,
            isEdit: false,
            secretKey: props.secretKey,
            onRemove: props.onRemove
        };

        this.handleEditClick = this.handleEditClick.bind(this);
        this.handleExpandClick = this.handleExpandClick.bind(this);
        this.onNoteEdit = this.onNoteEdit.bind(this);
        this.handleRemoveClick = this.handleRemoveClick.bind(this);

        this.restClient = new RestClient();

    }

    handleEditClick() {
        this.setState({ isEdit: !this.state.isEdit, isExapanded: true });
    }

    handleExpandClick() {
        this.setState({ isExapanded: !this.state.isExapanded });
    }

    onNoteEdit(noteFor, noteItem) {
        this.setState({ isEdit: false, noteItem: noteItem });
    }

    async handleRemoveClick(event) {

        try {

            await this.restClient.delete(`notes/${this.state.noteItem.id}`);
            
            if (this.state.onRemove) {
                this.state.onRemove(this);
            } else {
                console.warn('No handler for remove note.');
            }
            

        } catch(err) {
            console.warn(err);
        }
        
    }

    render() {

        let noteBodyId = 'note-body-' + this.state.noteItem.id;
        let cardClasses = 'card note-line';
        let cardBody = 'card-body collapse multi-collapse';

        if (this.state.isExapanded) {

            cardClasses += ' expanded';
            cardBody += ' show';

        }

        let noteBody = <p className="card-text">{this.state.noteItem.text}</p>

        if (this.state.isEdit) {
            noteBody = <NoteForm title={this.state.noteItem.title} onFormSend={this.onNoteEdit} text={this.state.noteItem.text} id={this.state.noteItem.id} secretKey={this.state.secretKey} />
        }

        return (<div className={cardClasses}>

            <div role="group" className="btn-group note-header-btns">
                <button onClick={this.handleExpandClick} type="button" data-toggle="collapse" data-target={`#${noteBodyId}`} className="btn btn-title">{this.state.noteItem.title}</button>
                <button className="btn btn-edit" onClick={this.handleEditClick}><i className="fas fa-edit"></i></button>
                <button className="btn btn-remove" onClick={this.handleRemoveClick}><i className="fas fa-trash"></i></button>
            </div>
            <div id={noteBodyId} className={cardBody}>
                {noteBody}
            </div>
        </div>)
    }

}

export default NoteLine;