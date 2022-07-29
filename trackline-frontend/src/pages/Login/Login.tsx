import React, { useState } from 'react'
import ILogin from '../../domain/ILogin'
import { IRestApiErrorResponse } from '../../domain/IRestApiErrorResponse';
import { IdentityService } from '../../services/IdentitySerivice'
import ErrorView from './ErrorView';
import FormView from './FormView'
import { useLoginUpdate } from "../../context/AuthStateContext";


const identityService = new IdentityService();

const Login = () => {
    const [values, setInput] = useState({
        email: "admin@itcollege.ee",
        password: "Password.1"
    } as ILogin);

    const [apiErrorResponse, setApiErrorResponse] = useState<IRestApiErrorResponse>({} as IRestApiErrorResponse)
    const [errorFlag, setErrorFlag] = useState(false)
    const loginUpdate = useLoginUpdate();

    const handleChange = (target:
        EventTarget & HTMLInputElement |
        EventTarget & HTMLSelectElement |
        EventTarget & HTMLTextAreaElement) => {
        //debugger;
        console.log(target.name, target.value, target.type, target)
    
        if (target.type === "text") {
            setInput({ ...values, [target.name]: target.value });
            return;
        }
        if (target.type === "checkbox") {
            setInput({ ...values, [target.name]: (target as EventTarget & HTMLInputElement).checked });
            return;
        }
    
        setInput({ ...values, [target.name]: target.value });
    };

    const loginButtonClicked = async (event : React.MouseEvent) => {        
        event.preventDefault();

        var response = await identityService.login(values.email, values.password)
        if (response.status >= 300) {
            setApiErrorResponse(response.data as IRestApiErrorResponse)
            setErrorFlag(true)
        } else{
            loginUpdate(true)
            setErrorFlag(false)
        }

    }
    
    return (
        <>
           {errorFlag ? <ErrorView apiErrorResponse={apiErrorResponse}/> : ''}
            <FormView loginModel={values} handleChange={handleChange} loginButtonClicked={loginButtonClicked}/>
        </>
    )
}

export default Login