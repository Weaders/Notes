import LogoutLink from "./logout-link.jsx";

class Header extends React.Component {
    render() {

        return (<nav className="navbar navbar-dark bg-dark navbar-fixed-top">
                    <a href="#" className="navbar-brand">Easy notes storage</a>
                    <LogoutLink />
                </nav>)

    }
}

export default Header;