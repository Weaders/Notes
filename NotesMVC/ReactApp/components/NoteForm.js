import PropTypes from 'prop-types'
import Errors from './Errors'

class NoteForm extends React.Component {

    static get propTypes() {

        return {
            title: PropTypes.string,
            text: PropTypes.string,
            id: PropTypes.number,
            errors: PropTypes.instanceOf(Map).isRequired,
            secretCode: PropTypes.string.isRequired,
            onFormSend: PropTypes.func.isRequired
        };

    }

    constructor(props) {

        super(props);

        this.state = {
            id: props.id || 0,
            title: props.title || '',
            text: props.text || '',
        };

        this.handleChangeInput = this.handleChangeInput.bind(this);
        this.handleClick = this.handleClick.bind(this);

    }

    handleChangeInput(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    async handleClick(event) {

        this.props.onFormSend(this);
        event.preventDefault();

    }

    render() {

        let btnText = this.state.id ? 'Edit' : 'Add';
        let diabled = this.props.secretCode.length == 0;

        return <form onSubmit={e => e.preventDefault()}>
            {!this.props.errors.size || <Errors errors={this.props.errors} />}
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