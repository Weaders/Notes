import { withLocalize } from 'react-localize-redux'
import cookies from 'react-cookies'

const LangSelectDropdown = ({ languages, activeLanguage, setActiveLanguage }) => {

  let langsHtml = [];

  for (let { code, name } of languages) {

    langsHtml.push((
      <a key={code} title={name} onClick={() => { cookies.save('CurrentLang', `c=${code}|uic=${code}`); setActiveLanguage(code); }} className = "dropdown-item" >
        <img className="flag-icon" src={`imgs/${code}.svg`} />
      </a >
    ));

  }

  if (activeLanguage) {

    return (<li className="nav-item dropdown">
      <a className="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <img className="flag-icon" src={`imgs/${activeLanguage.code}.svg`} />
      </a>
      <div className="dropdown-menu bg-dark lang-select" aria-labelledby="navbarDropdown">
        {langsHtml}
      </div>
    </li>);

  } else {
    return '';
  }

}

export default withLocalize(LangSelectDropdown);