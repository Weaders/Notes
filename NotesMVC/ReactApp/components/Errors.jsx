import PropTypes from 'prop-types';
import React from 'react';

const Errors = ({ errors }) => {
  const errorsHtml = Array.from(errors)
    .map(([key, error]) => (<p className="error-text" key={key} data-field={key}>{error}</p>));

  return (
    <div className="alert alert-danger">
      {errorsHtml}
    </div>
  );
};

Errors.propTypes = {
  errors: PropTypes.instanceOf(Map).isRequired,
};

export default Errors;
