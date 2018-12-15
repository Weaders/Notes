import RestClient from './rest-client';
import NoteItem from './note-item'

class NoteForm extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            id: props.id,
            title: props.title,
            text: props.text,
            secretKey: props.secretKey,
            onFormSend: props.onFormSend
        };

        this.handleChangeText = this.handleChangeText.bind(this);
        this.handleChangeTitle = this.handleChangeTitle.bind(this);
        this.handleClick = this.handleClick.bind(this);

        this.restClient = new RestClient();

    }

    handleChangeTitle(event) {

        this.setState({ title: event.target.value });
        event.preventDefault();

    }

    handleChangeText(event) {

        this.setState({ text: event.target.value });
        event.preventDefault();

    }

    async handleClick(event) {

        if (this.state.id) {

            let note = await this.restClient.post(`notes/${this.state.id}/edit`, { 
                text: this.state.text, 
                title: this.state.title, 
                id: this.state.id,
                secretKey: this.state.secretKey
            });

            this.state.onFormSend(this, new NoteItem(note.id, note.title, note.text));

        } else {

            let note = await this.restClient.post('notes/add', { 
                text: this.state.text, 
                title: this.state.title,
                secretKey: this.state.secretKey
            });

            this.state.onFormSend(this, new NoteItem(note.id, note.title, note.text));

        }

    }

    render() {

        let btnText = this.state.id ? 'Edit' : 'Add' ;

        return <form onSubmit={e => e.preventDefault()}>
            <div className="form-group">
                <label htmlFor="user">Title</label>
                <input value={this.state.title} id="title" className="form-control" onChange={this.handleChangeTitle} />
            </div>
            <div className="form-group">
                <label htmlFor="text">Text</label>
                <textarea value={this.state.text} id="text" className="form-control" onChange={this.handleChangeText} />
            </div>
            <button onClick={this.handleClick} className="btn btn-primary">{btnText}</button>
        </form>
    }

}

export default NoteForm;