import React, { useEffect, useState } from 'react';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';
import './Stats.css';

const Stats = () => {
  const [stats, setStats] = useState(null);

  const reactAppHost = process.env.REACT_APP_HOST || 'localhost';
  const reactAppPort = process.env.REACT_APP_PORT || '3000';

  const fetchStats = async () => {
    try {
      const response = await axios.get(`http://${reactAppHost}:${reactAppPort}/api/Trip/GetAllPreferences`);
      setStats(response.data);
      console.log(response.data);
    } catch (error) {
      console.error('Error fetching stats:', error.response || error.message);
    }
  };

  useEffect(() => {
    fetchStats();

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
        <p>Loading stats...</p>
      )}
    </div>
  );
};

export default Stats;
