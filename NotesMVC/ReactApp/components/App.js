import { Switch, withRouter } from 'react-router-dom'
import { Route } from 'react-router'

import propTypes from 'prop-types'

import AUserFrom from './../containers/AUserForm'
import UserNoteList from './../containers/UserNoteList'

import LoadingBox from './LoadingBox'
import UserHeader from './../containers/UserHeader'

class App extends React.Component {

    static get propTypes() {
        return {
            onStart: propTypes.func.isRequired,
            isLoading: propTypes.bool.isRequired
        }
    }

    componentDidMount() {
        this.props.onStart();
    }

    render() {

        let appClasses = [''];

        if (!this.props.isLoading) {
            appClasses.push('loading');
        }

        return (
            <div id="app" className={appClasses.join(' ')}>

                <UserHeader />
                <div className="container">
                    <Switch>
                        <Route exact path='/' component={AUserFrom} />
                        <Route path='/notes' component={UserNoteList} />
                    </Switch>
                </div>
                {!this.props.isLoading || <LoadingBox />}
                
            </div>
        )

    }

}

export default App