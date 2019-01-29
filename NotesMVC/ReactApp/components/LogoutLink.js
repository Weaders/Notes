import propTypes from 'prop-types'

const LogoutLink = ({ onClick }) => {
    return <a className="logout-link" onClick={() => { onClick() }}>Logout</a>
}

LogoutLink.propsTypes = {
    onClick: propTypes.func.isRequired
}

export default LogoutLink