import React from 'react';
import PropTypes from 'prop-types';
import UserLogoutLink from '../containers/UserLogoutLink';
import HeaderLangSelect from '../containers/HeaderLangSelect';

const Header = ({ isLogoutShow }) => (
  <nav className="navbar navbar-dark bg-dark navbar-fixed-top navbar-expand">
    <span className="site-title navbar-brand">Easy notes storage</span>
    <ul className="navbar-nav ml-auto">
      <HeaderLangSelect />
      {!isLogoutShow || <UserLogoutLink />}
    </ul>
  </nav>
);

Header.propTypes = {
  isLogoutShow: PropTypes.bool.isRequired,
};

export default Header;
