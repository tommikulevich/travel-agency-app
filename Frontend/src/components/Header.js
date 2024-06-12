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

  const handleStatsClick = () => {
    navigate('/stats');
  };

  return (
    <header>
      <Link to="/" className="site-title" onClick={onTitleClick}>
        RSWW - Twoja podróż to nasz projekt
      </Link>
      <span className="separator"></span>
      <div>
        {clientId ? (
          <>
            <button onClick={handleLogout} className="button-logout">
              Wyloguj
            </button>
            <span className="separator"></span>
            <Link to="/user-offers" className="button">
              Podgląd Twoich ofert
            </Link>
            <span className="separator"></span>
            {clientId === 'd8313a02-a0fd-4a13-8e3a-c22393c388a7' && (
              <button onClick={handleStatsClick} className="button">
                Statystyki dla nerdów
              </button>
            )}
          </>
        ) : (
          <Link to="/login" className="button">
            Zaloguj
          </Link>
        )}
      </div>
    </header>
  );
};

export default Header;
