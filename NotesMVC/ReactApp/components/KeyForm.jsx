import React from 'react';
import PropTypes from 'prop-types';
import { Translate } from 'react-localize-redux';

export default class KeyForm extends React.Component {
  static get propTypes() {
    return {
      translate: PropTypes.func.isRequired,
      onSubmit: PropTypes.func.isRequired,
      key: PropTypes.string,
    };
  }

  static get defaultProps() {
    return {
      key: '',
    };
  }

  constructor(props) {
    super(props);

    this.state = {
      key: props.key || '',
    };

    this.onChangeSecretKey = this.onChangeSecretKey.bind(this);
    this.onSubmit = this.onSubmit.bind(this);
  }

  /**
     * Call on change secret key
     * @param {Event} event
     */
  onChangeSecretKey(event) {
    this.setState({ key: event.target.value });

    event.preventDefault();
  }

  /**
     * Call on submit form
     */
  onSubmit(event) {
    const { onSubmit } = this.props;

    onSubmit(this);
    event.preventDefault();
  }

  render() {
    const { key } = this.state;
    const { translate } = this.props;

    return (
      <div className="input-group mb-3">

        <input itemType="text" value={key} className="form-control" onChange={this.onChangeSecretKey} placeholder={translate('SecretKey')} />
        <div className="input-group-append">
          <button className="btn btn-outline-secondary" onClick={this.onSubmit} type="button">
            <Translate id="Send" />
          </button>
        </div>
      </div>
    );
  }
}
