import React from 'react'
import ILogin from '../../domain/ILogin'

const FormView = (props: {
    loginModel: ILogin,
    handleChange: (target: (EventTarget & HTMLInputElement) | (EventTarget & HTMLSelectElement) | (EventTarget & HTMLTextAreaElement)) => void,
    loginButtonClicked: (e : React.MouseEvent) => void},
    ) => {

  return (
    <>
    
    <form>
        <div className="form-group">
            <label htmlFor="inputEmail">Email</label>
            <input value={props.loginModel.email} onChange={(e) => props.handleChange(e.target)} name="email" type="text" className="form-control" id="inputEmail" aria-describedby="emailHelp" />
        </div>

        <div className="form-group">
            <label htmlFor="inputPassword">Password</label>
            <input value={props.loginModel.password} onChange={(e) => props.handleChange(e.target)} name="password" type="text" className="form-control" id="inputPassword" aria-describedby="passwordHelp" />
        </div>
        <button className='button' onClick={(e) => props.loginButtonClicked(e)}>
            Login
        </button>
    </form>
    </>

  )
}

export default FormView