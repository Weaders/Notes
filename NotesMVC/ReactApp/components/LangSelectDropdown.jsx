import React from 'react';

const LangSelectDropdown = ({ languages, activeLanguage, setActiveLanguage }) => {
  const langsHtml = Array.from(languages).map(({ code, name }) => (
    <button key={code} tabIndex={0} type="button" title={name} onClick={() => setActiveLanguage(code)} className="btn-link dropdown-item">
      <img alt={code} className="flag-icon" src={`imgs/${code}.svg`} />
    </button>
  ));

  if (activeLanguage) {
    return (
      <li className="nav-item dropdown">
        <button className="btn-link nav-link dropdown-toggle" type="button" id="navbarDropdown" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <img alt={activeLanguage.code} className="flag-icon" src={`imgs/${activeLanguage.code}.svg`} />
        </button>
        <div className="dropdown-menu bg-dark lang-select" aria-labelledby="navbarDropdown">
          {langsHtml}
        </div>
      </li>
    );
  }

  return '';
};

export default LangSelectDropdown;
