import React from "react";
import { NavLink } from "react-router-dom";
import { useLogin, useLoginUpdate } from "../../context/AuthStateContext";
import "../../styles/HeaderStyle.css";

type Props = {};

const Header = (props: Props) => {
  const loginState = useLogin();
  const loginStateUpdate = useLoginUpdate();

  const login = loginState ? "in" : "out";

  return (
    <header>
      <nav className="navbar navbar-expand-lg bg-light">
        <div className="container-fluid">
          <a className="navbar-brand" href="#">
            Navbar
          </a>
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarNavAltMarkup"
            aria-controls="navbarNavAltMarkup"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
            <div className="navbar-nav">
            <ul className="navbar-nav flex-grow-1">
            <li className="nav-item">
                <NavLink className="nav-link" to={"headlist"}>HeadList</NavLink>
              </li>
              <li>
                <NavLink className="nav-link" to={"login"}>Login</NavLink>
              </li>
              <li>
                <NavLink className="nav-link" to={"/"}>Description</NavLink>
              </li>
              
              </ul>

            </div>
          </div>
        </div>
      </nav>
    </header>
  );
};

export default Header;
