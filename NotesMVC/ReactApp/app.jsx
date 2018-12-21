import "@babel/polyfill"

import { HashRouter, Switch } from 'react-router-dom'
import { Route, Redirect } from 'react-router'

import UserForm from './user-form.jsx'
import NotesList from './notes-list.jsx'
import Header from './header/header.jsx'

import history from './common/history-app'

import appData from './models/app-data'
import { EVENT_USER_LOGOUT } from './models/app-data'
import UserItem from "./models/user-item.js";

class App extends React.Component {

    constructor(props) {

        super(props);

        /**
         * @type {{isLoggin: boolean, user: UserItem}}
         */
        this.state = {
            isLoggin: false,
            user: null
        };

        appData.on(EVENT_USER_LOGOUT, () => {
            
            this.setState({
                isLoggin: false,
                user: null
            });

        });

        this.onLogin = this.onLogin.bind(this);
        this._notesRouteRender = this._notesRouteRender.bind(this);

    }

    /**
     * Called on login
     * @param {UserItem} userItem
     */
    onLogin(userItem) {

        this.setState({
            user: userItem,
            isLoggin: true
        });

        appData.user = this.state.user;

        history.push('/notes');

    }

    render() {

        return (
            <div id="app">
                <Header />
                <div className="container body-content" id="content">
                    <Switch>
                        <Route exact path='/' render={() => <UserForm getCurrentUser={true} onLogin={this.onLogin} />} />
                        <Route path='/notes' render={this._notesRouteRender} />
                    </Switch>
                </div>
            </div>)

    }

    _notesRouteRender() {

        if (!this.state.isLoggin) {
            return <Redirect to="/" />
        } else {
            return <NotesList />
        }

    }

}

ReactDOM.render(<HashRouter><App /></HashRouter>, document.getElementById("body"));