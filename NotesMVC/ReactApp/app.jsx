import "@babel/polyfill"

import { HashRouter, Switch  } from 'react-router-dom'
import { Route, Redirect } from 'react-router'

import UserForm from './user-form.jsx'
import NotesList from './notes-list.jsx'

import history from './history'

class App extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            isLoggin: false,
            user: '',
            key: ''
        };

        this.onLogin = this.onLogin.bind(this);
        this._notesRouteRender = this._notesRouteRender.bind(this);

    }

    /**
     * Called on login
     * @param {import("./user-form.jsx").UserForm} userForm
     */
    onLogin(userForm) {

        this.setState({
            user: userForm.state.user,
            key: userForm.state.pwd,
            isLoggin: true
        });

        history.push('/notes');
        
    }

    render() {

        return (<Switch><Route exact path='/' render={() => <UserForm onLogin={this.onLogin} />} />
            <Route path='/notes' render={this._notesRouteRender} /></Switch>)

    }

    _notesRouteRender() {

        if (!this.state.isLoggin) {
            return <Redirect to="/"/>
        } else {
            return <NotesList sercretKey={this.state.key} />
        }

    }

}

ReactDOM.render(<HashRouter><App /></HashRouter>, document.getElementById("content"));