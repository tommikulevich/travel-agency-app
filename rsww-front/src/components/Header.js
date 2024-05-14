import React from 'react';
import { Link } from 'react-router-dom';
import './Header.css';

function Header({ onTitleClick }) {
  return (
    <header>
<Link to="/" className="site-title" onClick={onTitleClick}>RSWW - Twoja podróż to nasz projekt</Link>
      <div>
        <Link to="/login" className="button">Logowanie</Link>
        <span className="separator"></span>
        <Link to="/register" className="button">Rejestracja</Link>
        <span className="separator"></span>
        <Link to="/user-offers" className="button">Podgląd Twoich ofert</Link>
      </div>
    </header>
  );
}

export default Header;