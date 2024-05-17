import React, { useContext } from 'react'; 
import { Link, useNavigate } from 'react-router-dom';
import { AppContext } from '../App';
import './Header.css';

const Header = ({ onTitleClick }) => {
  const { clientId, setClientId } = useContext(AppContext);
  const navigate = useNavigate(); 

  const handleLogout = () => {
    setClientId('');
    navigate('/'); 
  };

  return (
    <header>
      <Link to="/" className="site-title" onClick={onTitleClick}>
        RSWW - Twoja podróż to nasz projekt
      </Link>
      <span className="separator"></span>
      <div>
        {clientId ? (
          <button onClick={handleLogout} className="button-logout">
            Wyloguj
          </button>
        ) : (
          <Link to="/login" className="button">
            Zaloguj
          </Link>
        )}
        <span className="separator"></span>
        {clientId && (
          <Link to="/user-offers" className="button">
            Podgląd Twoich ofert
          </Link>
        )}
      </div>
    </header>
  );
};

export default Header;