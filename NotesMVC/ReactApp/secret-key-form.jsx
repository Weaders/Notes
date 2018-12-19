class SecretKeyForm extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            key: '',
            onSubmit: props.onSubmit || (() => { })
        };

        this._onChangeSecretKey = this._onChangeSecretKey.bind(this);
        this._onSubmit = this._onSubmit.bind(this);

    }

    /**
     * Call on change secret key
     * @param {Event} event 
     */
    _onChangeSecretKey(event) {

        this.setState({key: event.target.value});

        event.preventDefault();

    }

    /**
     * Call on submit form
     */
    _onSubmit() {
        this.state.onSubmit(this);
    }

    render() {
        return (<div className="input-group mb-3">
                    <input itemType="text" className="form-control" onChange={this._onChangeSecretKey} placeholder="Secret key"/>
                    <div className="input-group-append">
                        <button className="btn btn-outline-secondary" onClick={this._onSubmit} type="button">Send</button>
                    </div>
                </div>)

    }

}

export default SecretKeyForm;