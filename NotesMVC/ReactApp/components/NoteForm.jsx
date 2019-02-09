import React from 'react';
import PropTypes from 'prop-types';
import { Translate } from 'react-localize-redux';
import Errors from './Errors';

class NoteForm extends React.Component {
  static get propTypes() {
    return {
      title: PropTypes.string,
      text: PropTypes.string,
      id: PropTypes.number,
      errors: PropTypes.instanceOf(Map).isRequired,
      secretCode: PropTypes.string.isRequired,
      onFormSend: PropTypes.func.isRequired,
    };
  }

  static get defaultProps() {
    return {
      title: '',
      text: '',
      id: 0,
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
    const { onFormSend } = this.props;

    onFormSend(this);
    event.preventDefault();
  }

  render() {
    const { id, title, text } = this.state;
    const { secretCode, errors } = this.props;

    const btnText = id ? new Translate({ id: 'Edit' }) : new Translate({ id: 'Add' });
    const diabled = secretCode.length === 0;

    const titleIdInp = 'title';
    const textIdInp = 'text';

    return (
      <form onSubmit={e => e.preventDefault()}>
        {!errors.size || <Errors errors={errors} />}

        <div className="form-group">
          <label className="label-form " htmlFor={titleIdInp}>
            <Translate id="Title" />
            <input value={title} id={titleIdInp} name="title" className="form-control" onChange={this.handleChangeInput} />
          </label>
        </div>

        <div className="form-group">
          <label className="label-form" htmlFor={textIdInp}>
            <Translate id="Text" />
            <textarea value={text} id={textIdInp} name="text" className="form-control" onChange={this.handleChangeInput} />
          </label>
        </div>

        <button onClick={this.handleClick} type="submit" disabled={diabled} className="btn btn-primary">
          {btnText}
        </button>
      </form>
    );
  }
}

export default NoteForm;
