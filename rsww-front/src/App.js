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

  useEffect(() => {
    const fetchData = async () => {
      const result = await axios('api/Offers/GetAllOffers');
      setOffers(result.data);
    };

    fetchData();
  }, []);

  return (
    <Router>
      <div className="App">
        <Header />
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={<><SearchForm /><Offers offers={offers} /></>} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
