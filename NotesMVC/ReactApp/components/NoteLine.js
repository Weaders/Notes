import UserNoteForm from "./../containers/UserNoteForm";
import NoteItem from './../models/NoteItem';
import LoadingBox from './LoadingBox';

import React from 'react';
import PropTypes from 'prop-types';

class NoteLine extends React.Component {

    static get propTypes() {

        return {
            noteItem: PropTypes.instanceOf(NoteItem).isRequired,
            onRemove: PropTypes.func.isRequired,
            isLoading: PropTypes.bool.isRequired
        };

    }

    constructor(props) {

        super(props);

        this.state = {
            isEdit: false,
            isExpanded: this.props.isExapanded
        };


        this.bodyOfNote = React.createRef();

        this.handleEditClick = this.handleEditClick.bind(this);
        this.handleRemoveClick = this.handleRemoveClick.bind(this);

    }

    handleEditClick() {
        this.setState({ isEdit: !this.state.isEdit, isExpanded: true });
    }

    async handleRemoveClick(event) {
        this.props.onRemove(this);
    }

    componentDidMount() {

        $(this.bodyOfNote.current).on('shown.bs.collapse', () => {
            
            // this.setState({
            //     isExpanded: true
            // });

        }).on('hidden.bs.collapse', () => {
            
            // this.setState({
            //     isExpanded: false
            // });
            
        });

    }

    render() {

        let noteBodyId = 'note-body-' + this.props.noteItem.id;
        let cardClasses = 'card note-line';
        let cardBody = 'card-body collapse multi-collapse';
        let loadingBox = '';

        if (this.state.isExpanded) {

            // cardClasses += ' expanded';
            // cardBody += ' show';

            if (this.props.isLoading) {
                loadingBox = <LoadingBox />
            }

        }

        let noteBody = <p className="card-text">{this.props.noteItem.text}</p>

        if (this.state.isEdit) {
            noteBody = <UserNoteForm title={this.props.noteItem.title} text={this.props.noteItem.text} id={this.props.noteItem.id} />
        }

        return (<div className={cardClasses}>
            {loadingBox}
            <div role="group" className="btn-group note-header-btns card-header">
                <button type="button" data-toggle="collapse" data-target={`#${noteBodyId}`} className="btn btn-title">{this.props.noteItem.title}</button>
                <button className="btn btn-edit" onClick={this.handleEditClick}><i className="fas fa-edit"></i></button>
                <button className="btn btn-remove" onClick={this.handleRemoveClick}><i className="fas fa-trash"></i></button>
            </div>
            <div id={noteBodyId} ref={this.bodyOfNote} className={cardBody}>
                {noteBody}
            </div>
        </div>)
    }

}

export default NoteLine;