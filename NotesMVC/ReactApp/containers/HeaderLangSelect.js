import cookies from 'react-cookies';
import { connect } from 'react-redux';
import { setActiveLanguage, getActiveLanguage, getLanguages } from 'react-localize-redux';
import LangSelectDropdown from '../components/LangSelectDropdown';

const mapStateToProps = (state, ownProps) => ({
  ...ownProps,
  languages: getLanguages(state.localize),
  activeLanguage: getActiveLanguage(state.localize),
});

const mapDispatchToProps = (dispatch, ownProps) => Object.assign({
  setActiveLanguage: (langCode) => {
    cookies.save('CurrentLang', `c=${langCode}|uic=${langCode}`);
    return dispatch(setActiveLanguage(langCode));
  },
}, ownProps);

export default connect(mapStateToProps, mapDispatchToProps)(LangSelectDropdown);
