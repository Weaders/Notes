import UserLogoutLink from "./../containers/UserLogoutLink";
import PropTypes from 'prop-types'

const Header = (props) => {

    return (<nav className="navbar navbar-dark bg-dark navbar-fixed-top">
        <a href="#" className="navbar-brand">Easy notes storage</a>
        {!props.isLogoutShow || <UserLogoutLink/>}
    </nav>)

}

Header.propTypes = {
    isLogoutShow: PropTypes.bool.isRequired
}

export default Header;