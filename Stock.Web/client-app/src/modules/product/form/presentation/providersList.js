import React, { Component } from "react";
import api from "../../../../common/api"
import { Field } from "redux-form";
import SelectField from "../../../../components/inputs/SelectField";

class ProviderList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            options: []
        }
    }

    componentDidMount() {
        api.get("/provider").then(response => {
            this.setState({
                options: this.state.options.concat([{value: "",label: "Seleccione el proveedor"}])
            })
            const providers = response.data.map(type => {
                return {
                    value: type.id,
                    label: type.description
                }
            });
            this.setState({options:this.state.options.concat(providers)})
        })
    }

    render(){
        return(
            <Field label="Proveedores" options={this.state.options} name="providerId" component={SelectField} type="select"></Field>
        )
    }
}

export default ProviderList;