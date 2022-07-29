import React, { useEffect } from 'react';
import './styles/App.css';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import { LoginProvider, useLogin, useLoginUpdate } from './context/AuthStateContext';

import Header from './components/Layouts/Header';
import Footer from './components/Layouts/Footer';
import Login from './pages/Login/Login';
import HeadList from './pages/HeadList/HeadList';
import Description from './pages/Description/Description';
import { Cookies } from 'react-cookie';

const cookie = new Cookies;

function App() {

  const setAuthState = useLoginUpdate();
  
  useEffect(() => {
    console.log("checking cookies")
    console.log(cookie.get("jwt"))

    if (cookie.get("jwt") === undefined) {
      setAuthState(false)
    } else {
      console.log("setting true")
      setAuthState(true)
    }
  })

  return (
    <LoginProvider>
      <Header />
        <div className="container">
          <main className="pb-3" role="main">
            <Routes>
                <Route index element={<Description/>}/> 
                <Route path="login" element={<Login />}/>
                <Route path="headlist" element={<HeadList />}/>
            </Routes>
          </main>
        </div>
      <Footer/>
    </LoginProvider>
  );
}

export default App;
