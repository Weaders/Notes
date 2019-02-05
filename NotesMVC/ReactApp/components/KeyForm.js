import PropTypes from 'prop-types'
import { Translate } from 'react-localize-redux'

export default class KeyForm extends React.Component {

    static get propTypes() {

        return {
            translate: PropTypes.func.isRequired,
            onSubmit: PropTypes.func.isRequired,
            key: PropTypes.string
        };

    }

    constructor(props) {

        super(props);

        this.state = {
            key: props.key || ''
        };

        this.onChangeSecretKey = this.onChangeSecretKey.bind(this);
        this.onSubmit = this.onSubmit.bind(this);

    }

    /**
     * Call on change secret key
     * @param {Event} event 
     */
    onChangeSecretKey(event) {

        this.setState({ key: event.target.value });

        event.preventDefault();

    }

    /**
     * Call on submit form
     */
    onSubmit(event) {
        this.props.onSubmit(this);
        event.preventDefault();
    }

    render() {

        return (<div className="input-group mb-3">
            
            <input itemType="text" value={this.state.key} className="form-control" onChange={this.onChangeSecretKey} placeholder={this.props.translate("secretKey")} />
            <div className="input-group-append">
                <button className="btn btn-outline-secondary" onClick={this.onSubmit} type="button">
                    <Translate id="send" />
                </button>
            </div>
        </div>)

    }

}