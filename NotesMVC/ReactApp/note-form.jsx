import RestClient from './rest/rest-client';
import NoteItem from './models/note-item'
import ReqError from './rest/req-error';

import appData from './models/app-data'
import { EVENT_CODE_CHANGED } from './models/app-data'

class NoteForm extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            id: props.id || 0,
            title: props.title || '',
            text: props.text || '',
            onFormSend: props.onFormSend
        };

        this.handleChangeInput = this.handleChangeInput.bind(this);
        this.handleClick = this.handleClick.bind(this);

        appData.on(EVENT_CODE_CHANGED, () => {
            this.forceUpdate();
        });

        this.restClient = new RestClient();

    }

    handleChangeInput(event) {
        this.setState({[event.target.name]: event.target.value});
    }

    async handleClick(event) {

        let result = null;

        if (this.state.id) {

            result = await this.restClient.post(`notes/${this.state.id}/edit`, { 
                text: this.state.text, 
                title: this.state.title, 
                id: this.state.id,
                secretKey: appData.secretCode
            });

        } else {

            result = await this.restClient.post('notes/add', { 
                text: this.state.text, 
                title: this.state.title,
                secretKey: appData.secretCode
            });

        }

        if (result instanceof ReqError) {
            console.warn(result.errors);
        } else {
            this.state.onFormSend(this, new NoteItem(result.id, result.title, result.text));
        }

    }

    render() {

        let btnText = this.state.id ? 'Edit' : 'Add' ;
        let diabled = !appData.secretCode;

        return <form onSubmit={e => e.preventDefault()}>
            <div className="form-group">
                <label htmlFor="user">Title</label>
                <input value={this.state.title} id="title" name="title" className="form-control" onChange={this.handleChangeInput} />
            </div>
            <div className="form-group">
                <label htmlFor="text">Text</label>
                <textarea value={this.state.text} id="text" name="text" className="form-control" onChange={this.handleChangeInput} />
            </div>
            <button onClick={this.handleClick} disabled={diabled} className="btn btn-primary">{btnText}</button>
        </form>
    }

}

export default NoteForm;