import "@babel/polyfill"

import { HashRouter } from 'react-router-dom'

import thunkMiddleware from 'redux-thunk'
import { createLogger } from 'redux-logger'
import { createStore, applyMiddleware } from 'redux'

import reducers from './reducers'
import { render } from 'react-dom'
import { Provider } from 'react-redux'
import UserApp from './containers/UserApp'

import authRouteMiddleware from './middleware/authRoute'

const logger = createLogger();

const store = createStore(reducers, applyMiddleware(authRouteMiddleware, thunkMiddleware, logger));

render(
    <Provider store={store}>
        <HashRouter>
            <UserApp />
        </HashRouter>
    </Provider>,
    document.getElementById("body")
);