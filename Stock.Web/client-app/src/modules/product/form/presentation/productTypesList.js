import React, { Component } from "react";
import api from "../../../../common/api"
import { Field } from "redux-form";
import SelectField from "../../../../components/inputs/SelectField";

class ProductTypeList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            options: []
        }
    }

    componentDidMount() {
        api.get("/productType").then(response => {
            this.setState({
                options: this.state.options.concat([{value: "",label: "Seleccione la categoría"}])
            })
            const productTypes = response.data.map(type => {
                return {
                    value: type.id,
                    label: type.description
                }
            });
            this.setState({options:this.state.options.concat(productTypes)})
        })
    }

    render(){
        return(
            <Field label="Categorías" options={this.state.options} name="productTypeId" component={SelectField} type="select"></Field>
        )
    }
}

export default ProductTypeList;