import UserLogoutLink from "./../containers/UserLogoutLink";
import LangSelectDropdown from './LangSelectDropdown'
import PropTypes from 'prop-types'

const Header = (props) => {

    return (<nav className="navbar navbar-dark bg-dark navbar-fixed-top navbar-expand">
        <a href="#" className="navbar-brand">Easy notes storage</a>
        <ul className="navbar-nav ml-auto">
            <LangSelectDropdown />
            {!props.isLogoutShow || <UserLogoutLink/>}
        </ul>
    </nav>)

}

Header.propTypes = {
    isLogoutShow: PropTypes.bool.isRequired
}

export default Header;