import PropTypes from 'prop-types'

const Errors = ({ errors }) => {

  let errorsHtml = []

  for (let [key, error] of errors) {
    errorsHtml.push(<p className="error-text" key={key} data-field={key}>{error}</p>);
  }

  return (<div className="alert alert-danger">
    {errorsHtml}
  </div>);

};

Errors.propTypes = {
  errors: PropTypes.instanceOf(Map).isRequired
};

export default Errors;