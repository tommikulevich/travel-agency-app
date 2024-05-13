import React from 'react';
import { Link } from 'react-router-dom';
import './Header.css';

function Header() {
  return (
    <header>
      <Link to="/" className="site-title">RSWW - Twoja podróż to nasz projekt</Link>
      <div>
        <Link to="/login" className="button">Logowanie</Link>
        <span className="separator"></span>
        <Link to="/register" className="button">Rejestracja</Link>
      </div>
    </header>
  );
}

export default Header;
