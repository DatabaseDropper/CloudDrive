import React from 'react';

class Component_Form_AbstractForm extends React.Component {
    constructor(props) {
        super(props);

        this.state = {submitProcess: false, fields: {}, errors: []};

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        let newStateFields = this.state.fields;
        newStateFields[event.target.name] = (event.target.type === 'checkbox' ? event.target.checked : event.target.value);
        this.setState({ fields: newStateFields });
    }
    
    handleSubmit() {
    }
}

export default Component_Form_AbstractForm;