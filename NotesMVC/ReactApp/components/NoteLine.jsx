import React from 'react';
import PropTypes from 'prop-types';
import UserNoteForm from '../containers/UserNoteForm';
import NoteItem from '../models/NoteItem';
import LoadingBox from './LoadingBox';


class NoteLine extends React.Component {
  static get propTypes() {
    return {
      noteItem: PropTypes.instanceOf(NoteItem).isRequired,
      onRemove: PropTypes.func.isRequired,
      isLoading: PropTypes.bool.isRequired,
      isExpanded: PropTypes.bool.isRequired,
    };
  }

  constructor(props) {
    super(props);

    const { isExpanded } = this.props;

    this.state = {
      isEdit: false,
      isExpanded,
    };


    this.bodyOfNote = React.createRef();

    this.handleEditClick = this.handleEditClick.bind(this);
    this.handleRemoveClick = this.handleRemoveClick.bind(this);
  }

  componentDidMount() {
    $(this.bodyOfNote.current).on('shown.bs.collapse', () => {
      this.setState({
        isExpanded: true,
      });
    }).on('hidden.bs.collapse', () => {
      this.setState({
        isExpanded: false,
      });
    });
  }

  async handleRemoveClick() {
    const { onRemove } = this.props;
    onRemove(this);
  }

  handleEditClick() {
    this.setState(({ isEdit }) => ({
      isEdit: !isEdit,
      isExpanded: true,
    }));
  }

  render() {
    const { noteItem, isLoading } = this.props;
    const { isExpanded, isEdit } = this.state;


    const noteBodyId = `note-body-${noteItem.id}`;
    let cardClasses = 'card note-line';
    let cardBody = 'card-body collapse multi-collapse';
    let loadingBox = '';

    if (isExpanded) {
      cardClasses += ' expanded';
      cardBody += ' show';

      if (isLoading) {
        loadingBox = <LoadingBox />;
      }
    }

    let noteBody = <p className="card-text">{noteItem.text}</p>;

    if (isEdit) {
      noteBody = <UserNoteForm title={noteItem.title} text={noteItem.text} id={noteItem.id} />;
    }

    return (
      <div className={cardClasses}>
        {loadingBox}
        <div role="group" className="btn-group note-header-btns card-header">
          <button type="button" data-toggle="collapse" data-target={`#${noteBodyId}`} className="btn btn-title">{noteItem.title}</button>
          <button className="btn btn-edit" type="button" onClick={this.handleEditClick}>
            <i className="fas fa-edit" />
          </button>
          <button className="btn btn-remove" type="button" onClick={this.handleRemoveClick}>
            <i className="fas fa-trash" />
          </button>
        </div>
        <div id={noteBodyId} ref={this.bodyOfNote} className={cardBody}>
          {noteBody}
        </div>
      </div>
    );
  }
}

export default NoteLine;
