import RestClient from './rest/rest-client'
import ReqError from './rest/req-error'
import UserItem from './models/user-item';

class UserForm extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            user: '',
            pwd: '',
            errors: new Map(),
            onLogin: props.onLogin || (() => { }),            
            loading: false
        };

        this.handleChangePassword = this.handleChangePassword.bind(this);
        this.handleChangeUser = this.handleChangeUser.bind(this);
        this.handleLogin = this.handleLogin.bind(this);
        this.handleRegister = this.handleRegister.bind(this);

        this.restClient = new RestClient();

    }

    componentDidMount() {
        
        if (this.props.getCurrentUser) {
            this.tryGetUser();
        }

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

        let data = null;

        try {

            data = await this.restClient.post('user/login', {
                user: this.state.user,
                password: this.state.pwd
            });

        } catch (reqError) {
            
            this.setState({ errors: reqError.errors });
            return;

        }

        if (this.state.onLogin) {

            let userItem = new UserItem();

            userItem.id = data.id;
            userItem.username = data.username;

            this.state.onLogin(userItem);

        }

    }

    async handleRegister(event) {

        event.preventDefault();

        let data = null;

        try {

            data = await this.restClient.post('user/register', {
                user: this.state.user,
                password: this.state.pwd
            });

        } catch (reqError) {
            this.setState({ errors: reqError.errors })
        }

        if (this.state.onLogin) {

            let userItem = new UserItem();

            userItem.id = data.id;
            userItem.username = data.username;
            
            this.state.onLogin(userItem);

        }

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

    handleSubmit() {
        event.preventDefault();
        return false;
    }

    render() {

        let errors = [];
        let errorEle = '';

        for (let [key, error] of this.state.errors) {
            errors.push(<p className="error-text" key={key} data-field={key}>{error}</p>);
        }

        if (errors.length) {

            errorEle = <div className="alert alert-danger">
                {errors}
            </div>

        }

        return <form className="user-form" onSubmit={e => e.preventDefault()}>
            {errorEle}
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