import React, { useContext, useEffect, useState } from 'react'; // Importuj useState
import { Link, useNavigate } from 'react-router-dom';
import { AppContext } from '../App';
import './Header.css';

function Header({ onTitleClick }) {
  const { clientId, setClientId } = useContext(AppContext);
  const navigate = useNavigate(); // Przenieś useNavigate() tutaj

  const handleLogout = () => {
    console.log('LOGOUTHANDLED', 0); 
    setClientId(''); // Zeruj clientId
    console.log('clientId został zresetowany');
    navigate('/'); // Przekieruj do strony głównej
  };
  

  return (
    <header>
      <Link to="/" className="site-title" onClick={onTitleClick}>RSWW - Twoja podróż to nasz projekt</Link>
      <span className="separator"></span>
      <div>
      {clientId ? (
  <button onClick={handleLogout} className="button-logout">Wyloguj</button>
) : (
  
  <Link to="/login" className="button">Zaloguj</Link>
)}

        {/* <span className="separator"></span>
        <Link to="/register" className="button">Rejestracja</Link> */}
        <span className="separator"></span>
        {clientId && (
  <Link to="/user-offers" className="button">Podgląd Twoich ofert</Link>
)}

      </div>
    </header>
  );
}

export default Header;
