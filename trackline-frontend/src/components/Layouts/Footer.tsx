import React from 'react'
import '../../styles/EyeAnimation.css'


import { useLogin } from '../../context/LoginStateContext'



const Footer = () => {
  const LoginContext = useLogin();  
  return (
    <>
        <div>{LoginContext ? "adsf" : "adf"}</div>
            <div className="container">
      <div className="eyeBall">
        <div className="iris"></div>
      </div>
      <div className="eyeLid"></div>
      <div className="lid"></div>
    </div>
    </>

  )
}

export default Footer
