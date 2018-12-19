import RestClient from './rest/rest-client'

class UserForm extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            user: '',
            pwd: '',
            errors: new Map()
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

        if (this.props.onLogin) {
            this.props.onLogin(this);
        }

    }

    async handleRegister(event) {

        event.preventDefault();

        try {

            let data = await this.restClient.post('user/register', {
                user: this.state.user,
                password: this.state.pwd
            });

            if (this.props.onLogin) {
                this.props.onLogin(this);
            }

        } catch (reqError) {
            this.setState({ errors: reqError.errors })
        }

    }


    handleSubmit() {
        event.preventDefault();
        return false;
    }

    render() {

        let errors = [];
        let errorEle = '';

        for (let [key, error] of this.state.errors) {
            errors.push(<p className="error-text" data-field={key}>{error}</p>);
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