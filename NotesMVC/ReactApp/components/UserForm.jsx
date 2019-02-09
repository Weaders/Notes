import React from 'react';
import PropTypes from 'prop-types';
import { Translate } from 'react-localize-redux';
import RestClient from '../rest/rest-client';
import Error from './Errors';

class UserForm extends React.Component {
  static get propTypes() {
    return {
      onLoginClick: PropTypes.func.isRequired,
      onRegisterClick: PropTypes.func.isRequired,
      errors: PropTypes.instanceOf(Map),
    };
  }

  static get defaultProps() {
    return {
      errors: new Map(),
    };
  }

  constructor(props) {
    super(props);

    this.state = {
      user: '',
      pwd: '',
    };

    this.handleChangePassword = this.handleChangePassword.bind(this);
    this.handleChangeUser = this.handleChangeUser.bind(this);
    this.handleLogin = this.handleLogin.bind(this);
    this.handleRegister = this.handleRegister.bind(this);

    this.restClient = new RestClient();
  }

  handleChangeUser(event) {
    this.setState({ user: event.target.value });
    event.preventDefault();
  }

  handleChangePassword(event) {
    this.setState({ pwd: event.target.value });
    event.preventDefault();
  }

  async handleLogin(event) {
    const { onLoginClick } = this.props;
    const { pwd, user } = this.state;

    event.preventDefault();
    onLoginClick(user, pwd);
  }

  async handleRegister(event) {
    const { onRegisterClick } = this.props;
    const { pwd, user } = this.state;

    event.preventDefault();
    onRegisterClick(user, pwd);
  }

  render() {
    const { errors } = this.props;
    const { user, password } = this.state;

    return (
      <form className="user-form" onSubmit={e => e.preventDefault()}>
        {!errors.size || <Error errors={errors} />}
        <div className="form-group">
          <label htmlFor="user" className="label-form">
            <Translate id="User" />
            <input value={user} id="user" className="form-control" onChange={this.handleChangeUser} />
          </label>
        </div>
        <div className="form-group">
          <label htmlFor="password" className="label-form">
            <Translate id="Password" />
            <input type="password" value={password} id="password" className="form-control" onChange={this.handleChangePassword} />
          </label>
        </div>
        <div className="user-form-buttons">
          <button onClick={this.handleLogin} type="submit" className="btn btn-primary">
            <Translate id="Login" />
          </button>
          <button onClick={this.handleRegister} type="submit" className="btn btn-second">
            <Translate id="Register" />
          </button>
        </div>
      </form>
    );
  }
}

export default UserForm;
