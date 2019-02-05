import { Route, Switch } from 'react-router'
import { renderToStaticMarkup } from "react-dom/server";

import propTypes from 'prop-types'

import AUserFrom from './../containers/AUserForm'
import UserNoteList from './../containers/UserNoteList'

import LoadingBox from './LoadingBox'
import UserHeader from './../containers/UserHeader'

import enGlobal from './../translation/en.global.json'
import ruGlobal from './../translation/ru.global.json'

class App extends React.Component {

    constructor(props) {

        super(props);

        this.props.initialize({
            languages: [
                { name: 'Russian', code: 'ru' },
                { name: 'English', code: 'en' }
            ],
            translation: {},
            options: { renderToStaticMarkup }
        });

        this.props.addTranslationForLanguage(enGlobal, 'en');
        this.props.addTranslationForLanguage(ruGlobal, 'ru');

    }

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