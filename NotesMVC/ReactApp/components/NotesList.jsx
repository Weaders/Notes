import React from 'react';
import { Translate } from 'react-localize-redux';
import PropTypes from 'prop-types';
import UserNoteForm from '../containers/UserNoteForm';
import UserNoteLine from '../containers/UserNoteLine';
import SecretKeyForm from '../containers/SecretKeyForm';
import LoadingBox from './LoadingBox';
import NoteItem from '../models/NoteItem';

class NotesList extends React.Component {
  static get propTypes() {
    return {
      notes: PropTypes.arrayOf(PropTypes.instanceOf(NoteItem)).isRequired,
      getNotes: PropTypes.func.isRequired,
      isLoading: PropTypes.bool.isRequired,
    };
  }

  componentDidMount() {
    const { getNotes } = this.props;
    getNotes();
  }

  /**
   * Get form for add new note.
   */
  static getAddForm() {
    return (
      <div className="card">
        <div className="card-header">
          <Translate id="Add" />
        </div>
        <div className="card-body">
          <UserNoteForm />
        </div>
      </div>
    );
  }

  render() {
    const { isLoading, notes } = this.props;
    const result = notes.map(note => (<UserNoteLine key={`${note.id}`} noteItem={note} />));

    return (
      <div className="note-list">
        {!isLoading || <LoadingBox />}
        <SecretKeyForm />
        {result}
        {NotesList.getAddForm()}
      </div>
    );
  }
}

export default NotesList;
