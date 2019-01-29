import { combineReducers } from 'redux'
import user from './user'
import notes from './notes'
import secretCode from './secret-code'

export default combineReducers({ user, notes, secretCode });