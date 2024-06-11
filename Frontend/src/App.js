import React, { createContext, useCallback, useEffect, useState } from 'react';
import axios from 'axios';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import * as signalR from '@microsoft/signalr';
import Header from './components/Header';
import SearchForm from './components/SearchForm';
import Offers from './components/Offers';
import ReservedOffers from './components/ReservedOffers';
import Login from './components/Login';
import Register from './components/Register';
import './App.css';

export const AppContext = createContext();

const App = () => {
  const [offers, setOffers] = useState([]);
  const [clientId, setClientId] = useState(null);

  const reactAppHost = process.env.REACT_APP_HOST || 'localhost';
  const reactAppPort = process.env.REACT_APP_PORT || 3000;

  const fetchData = useCallback(async () => {
    try {
      const result = await axios(`http://${reactAppHost}:${reactAppPort}/api/Trip/GetAllTrips`);
      setOffers(result.data);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  }, [reactAppHost, reactAppPort]); 

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const handleSearch = async (searchParams) => {
    try {
      const result = await axios.get(`http://${reactAppHost}:${reactAppPort}/api/Trip/GetTripsByPreferences`, {
        params: searchParams,
      });
      setOffers(result.data);
    } catch (error) {
      console.error('Error searching offers:', error);
    }
  };

  // SignalR connection setup
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`http://${reactAppHost}:${reactAppPort}/notificationHub`)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection.on("ReceiveMessage", (message) => {
      console.log("Received message:", message);
    });

    connection.start()
      .then(() => console.log("SignalR Connected"))
      .catch((err) => console.error("SignalR Connection Error:", err));

    // Clean up the connection on component unmount
    return () => {
      connection.stop().then(() => console.log("SignalR Disconnected"));
    };
  }, [reactAppHost, reactAppPort]);

  return (
    <AppContext.Provider value={{ clientId, setClientId }}>
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
          <footer style={{ textAlign: 'left', padding: '2px', fontSize: '8px' }}>
            SK, TM, OP, MK, Grafiki: pix4free.org
          </footer>
        </div>
      </Router>
    </AppContext.Provider>
  );
}

export default App;
