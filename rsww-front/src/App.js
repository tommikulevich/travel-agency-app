import React, { createContext, useEffect, useState } from 'react';
import axios from 'axios';
import { BrowserRouter as Router, Route, Routes, useNavigate } from 'react-router-dom';
import Header from './components/Header';
import SearchForm from './components/SearchForm';
import Offers from './components/Offers';
import ReservedOffers from './components/ReservedOffers'; // Upewnij się, że ten import jest poprawny
import Login from './components/Login';
import Register from './components/Register';
import './App.css';

export const AppContext = createContext();

function App() {
  const [offers, setOffers] = useState([]);
  const [clientId, setClientId] = useState(null); // Dodaj to, jeśli nie masz już clientId

  const reactAppHost = process.env.REACT_APP_HOST;
  const reactAppPort = process.env.REACT_APP_PORT;
  

  const fetchData = async () => {
    const result = await axios(`http://${reactAppHost}:${reactAppPort}/api/Trip/GetAllTrips`);
    setOffers(result.data);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleSearch = async (searchParams) => {
    const result = await axios.get(`http://${reactAppHost}:${reactAppPort}/api/Trip/GetTripsByPreferences`, {
      params: searchParams,
    });

    setOffers(result.data);
  };

  return (
    <AppContext.Provider value={{ clientId, setClientId }}> {/* Usuń navigate z kontekstu */}
      <Router>
        <div className="App">
          <Header onTitleClick={fetchData} />
          <div style={{ flex: 1 }}>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/user-offers" element={<ReservedOffers />} />
            <Route path="/" element={<><SearchForm onSearch={handleSearch} /><Offers offers={offers} /></>} />
          </Routes>
        </div>
        <footer style={{ textAlign: 'left', padding: '2px', fontSize: '8px' }}>SK, TM, OP, MK,    Grafiki: pix4free.org</footer>
        </div>
      </Router>
    </AppContext.Provider>
  );
}

export default App;
