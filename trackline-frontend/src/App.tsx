import React from 'react';
import './App.css';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import { LoginProvider } from './context/LoginStateContext';

import Header from './components/Layouts/Header';
import Footer from './components/Layouts/Footer';

function App() {
  return (
    <LoginProvider>
      <Header />
        <div className="container">
          <main className="pb-3" role="main">
            <Routes>
                <Route />
            </Routes>
          </main>
        </div>
      <Footer/>
    </LoginProvider>
  );
}

export default App;
