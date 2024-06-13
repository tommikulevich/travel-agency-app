import React, { useEffect, useState } from 'react';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';
import './Stats.css';

const Stats = () => {
  const [stats, setStats] = useState(null);
  const [isGenerating, setIsGenerating] = useState(false);
  const [lastReservedOffers, setLastReservedOffers] = useState([]);

  const reactAppHost = process.env.REACT_APP_HOST || 'localhost';
  const reactAppPort = process.env.REACT_APP_PORT || '3000';

  const fetchStats = async () => {
    try {
      const response = await axios.get(`http://${reactAppHost}:${reactAppPort}/api/Trip/GetAllPreferences`);
      setStats(response.data);
      console.log('Stats:', response.data);
    } catch (error) {
      console.error('Error fetching stats:', error.response || error.message);
    }
  };

  const fetchGenerationState = async () => {
    try {
      //const response = await axios.get(`http://${reactAppHost}:${reactAppPort}/api/Trip/GetGenerationState`);
      setIsGenerating(response.data.isGenerating);
      console.log('Generation state:', response.data.isGenerating);
    } catch (error) {
      console.error('Error fetching generation state:', error.response || error.message);
    }
  };

  const toggleGeneration = async () => {
    try {
      let response;
      if (isGenerating) {
        response = await axios.post(`http://${reactAppHost}:${reactAppPort}/api/Trip/StopGeneration`);
        setIsGenerating(false);
      } else {
        response = await axios.post(`http://${reactAppHost}:${reactAppPort}/api/Trip/StartGeneration`);
        setIsGenerating(true);
      }
      console.log('Toggle response:', response.data);
    } catch (error) {
      console.error('Error toggling generation:', error.response || error.message);
    }
  };

  useEffect(() => {
    fetchStats();
    fetchGenerationState();

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`http://${reactAppHost}:${reactAppPort}/notificationHub`)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection.on("hotel", (message) => {
      console.log("Received message type: hotel, payload:", message);
      const newPreference = message.replace('\n is new preference', '');
      setStats(prevStats => ({ ...prevStats, hotelPreference: newPreference }));
    });

    connection.on("room", (message) => {
      console.log("Received message type: room, payload:", message);
      const newPreference = message.replace('\n is new preference', '');
      setStats(prevStats => ({ ...prevStats, roomPreference: newPreference }));
    });

    connection.on("destination", (message) => {
      console.log("Received message type: destination, payload:", message);
      const newPreference = message.replace('\n is new preference', '');
      setStats(prevStats => ({ ...prevStats, destinationPreference: newPreference }));
    });

    connection.on("transport", (message) => {
      console.log("Received message type: transport, payload:", message);
      const newPreference = message.replace('\n is new preference', '');
      setStats(prevStats => ({ ...prevStats, transportPreference: newPreference }));
    });

    connection.on("change", (message) => {
      console.log("Received message type: change, payload:", message);
      try {
        const parts = message.split(';');
        if (parts.length === 4) {
          const [offerID, changedType, changedValue, previousValue] = parts;
          const newChange = {
            offerID,
            changedType,
            changedValue,
            previousValue,
            timestamp: new Date().toISOString()
          };

          setLastReservedOffers(prevOffers => {
            const updatedOffers = [newChange, ...prevOffers];
            if (updatedOffers.length > 10) {
              updatedOffers.pop();
            }
            console.log('Updated offers:', updatedOffers);
            return updatedOffers;
          });
        } else {
          console.error('Unexpected message format:', message);
        }
      } catch (error) {
        console.error('Error parsing change message:', error.message);
      }
    });

    connection.start()
      .then(() => console.log("SignalR Connected"))
      .catch((err) => console.error("SignalR Connection Error:", err));

    return () => {
      connection.stop().then(() => console.log("SignalR Disconnected"));
    };
  }, [reactAppHost, reactAppPort]);

  return (
    <div className="stats-page">
      <h2>Statystyki dla nerdów</h2>
      {stats ? (
        <div className="stats-container">
          <p><strong>Preferencje</strong></p>
          <p><strong>Destynacja:</strong> {stats.destinationPreference}</p>
          <p><strong>Hotel:</strong> {stats.hotelPreference}</p>
          <p><strong>Pokój:</strong> {stats.roomPreference}</p>
          <p><strong>Transport:</strong> {stats.transportPreference}</p>
        </div>
      ) : (
        <p>Ładowanie statystyk...</p>
      )}
      <div className="generation-control">
        <button onClick={toggleGeneration} className="button">
          {isGenerating ? 'Stop generowania' : 'Generuj zmiany'}
        </button>
        <p>Status: {isGenerating ? 'Generowanie włączone' : 'Generowanie wyłączone'}</p>
      </div>
      <div className="last-reserved-offers-container">
        <h3>Ostatnie 10 zmienionych ofert</h3>
        <div className="last-reserved-offers">
          {lastReservedOffers.length > 0 ? (
            <ul>
              {lastReservedOffers.map((offer, index) => (
                <li key={index}>
                  <p><strong>ID ofert:</strong> {offer.offerID}</p>
                  <p><strong>Typ zmiany:</strong> {offer.changedType}</p>
                  <p><strong>Zmieniona wartość:</strong> {offer.changedValue}</p>
                  <p><strong>Poprzednia wartość:</strong> {offer.previousValue}</p>
                  <p><strong>Sygnatura czasu:</strong> {offer.timestamp}</p>
                </li>
              ))}
            </ul>
          ) : (
            <p>Brak zmian, póki co.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default Stats;
