import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Header from './components/Header';
import SearchForm from './components/SearchForm';
import Offers from './components/Offers';
import Login from './components/Login';
import Register from './components/Register';
import './App.css';

function App() {
  const [offers, setOffers] = useState([]);

  const fetchData = async () => {
    const result = await axios('http://localhost:8080/api/Trip/GetAllTrips');
    setOffers(result.data);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleSearch = async (searchParams) => {
    const result = await axios.get('http://localhost:8080/api/Trip/GetTripsByPreferences', {
      params: searchParams,
    });

    setOffers(result.data);
  };

  return (
    <Router>
      <div className="App">
        <Header onTitleClick={fetchData} />
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={<><SearchForm onSearch={handleSearch} /><Offers offers={offers} /></>} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
