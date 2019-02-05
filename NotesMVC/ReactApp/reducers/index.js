import { combineReducers } from 'redux'
import user from './user'
import notes from './notes'
import secretCode from './secret-code'
import { localizeReducer } from 'react-localize-redux'

export default combineReducers({
  user,
  notes,
  secretCode,
  localize: localizeReducer,
});