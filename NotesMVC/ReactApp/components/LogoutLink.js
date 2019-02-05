import propTypes from 'prop-types'
import { Translate } from "react-localize-redux"

const LogoutLink = ({ onClick }) => {
    return <li className={"nav-item"}>
        <a className="logout-link nav-link" onClick={() => { onClick() }}>
            <Translate id="logout" />
        </a>
    </li>
}

LogoutLink.propsTypes = {
    onClick: propTypes.func.isRequired
}

export default LogoutLink