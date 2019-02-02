import RestClient from '../rest/rest-client'
import UserItem from '../models/UserItem';
import PropTypes from 'prop-types'
import Error from './Errors'

class UserForm extends React.Component {

    static get propTypes () {
        
        return {    
            onLoginClick: PropTypes.func.isRequired,
            onRegisterClick: PropTypes.func.isRequired,
            isLoading: PropTypes.bool,
            errors: PropTypes.instanceOf(Map)
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

        event.preventDefault();

        this.props.onLoginClick(this);

    }

    async handleRegister(event) {

        event.preventDefault();

        this.props.onRegisterClick(this);

    }

    async tryGetUser() {

        this.setState({loading: true});

        let data = null;

        try {
            data = await this.restClient.get('user/current');
        } catch (reqError) {

            if (reqError.status != 304) {
                console.warn(reqError.errors);
            }
            
            return;
        }
        
        let userItem = new UserItem();

        userItem.id = data.id;
        userItem.username = data.username;

        if (this.state.onLogin) {
            this.state.onLogin(userItem);
        }

        this.setState({loading: false});

    }

    render() {

        return <form className="user-form" onSubmit={e => e.preventDefault()}>
            {!this.props.errors.size || <Error errors={this.props.errors} />}
            <div className="form-group">
                <label htmlFor="user">User</label>
                <input value={this.state.user} id="user" className="form-control" onChange={this.handleChangeUser} />
            </div>
            <div className="form-group">
                <label htmlFor="password">Password</label>
                <input type="password" value={this.state.password} id="password" className="form-control" onChange={this.handleChangePassword} />
            </div>
            <div className="user-form-buttons">
                <button onClick={this.handleLogin} className="btn btn-primary">Login</button>
                <button onClick={this.handleRegister} className="btn btn-second">Regiter</button>
            </div>
        </form>

    }

}

export default UserForm;