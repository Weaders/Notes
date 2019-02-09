import React from 'react';
import propTypes from 'prop-types';
import { Translate } from 'react-localize-redux';

const LogoutLink = ({ onClick }) => (
  <li className="nav-item">
    <button type="button" tabIndex={0} className="logout-link nav-link btn-link" onClick={() => { onClick(); }}>
      <Translate id="Logout" />
    </button>
  </li>
);

LogoutLink.propTypes = {
  onClick: propTypes.func.isRequired,
};

export default LogoutLink;
