import React from "react";
import "../../styles/EyeAnimation.css";

import { useLogin } from "../../context/AuthStateContext";

const Footer = () => {
  const LoginContext = useLogin();
  return (
    <>
      <div className="container fixed-bottom">
        <div>Footer: Login State: {LoginContext ? "LoggedIn" : "LoggedOut"} </div>
        <br/>
      </div>
    </>
  );
};

export default Footer;
