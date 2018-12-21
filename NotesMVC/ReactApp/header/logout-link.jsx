import appData from '../models/app-data'

class LogoutLink extends React.Component {

    render() {
        return <a className="logout-link" onClick={() => {appData.logout()}}>Logout</a>
    }

}

export default LogoutLink