import LangSelectDropdown from '../components/LangSelectDropdown'

import cookies from 'react-cookies'
import { connect } from 'react-redux'
import { setActiveLanguage, getActiveLanguage, getLanguages } from 'react-localize-redux'

const mapStateToProps = function (state, ownProps) {

  return {
    ...ownProps,
    languages: getLanguages(state.localize),
    activeLanguage: getActiveLanguage(state.localize) 
  };
  
}

const mapDispatchToProps = (dispatch, ownProps) => {

  return Object.assign({
    setActiveLanguage: (langCode) => {

      cookies.save('CurrentLang', `c=${langCode}|uic=${langCode}`); 
      return dispatch(setActiveLanguage(langCode));
      
    }
  }, ownProps);

}

export default connect(mapStateToProps, mapDispatchToProps)(LangSelectDropdown)