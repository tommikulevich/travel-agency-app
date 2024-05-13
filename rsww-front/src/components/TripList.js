import React, { useEffect, useState } from 'react';
import axios from 'axios';

function TripList() {
  const [trips, setTrips] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const result = await axios('api/Trip/GetAllTrips');
      setTrips(result.data);
    };

    fetchData();
  }, []);

  return (
    <ul>
      {trips.map(trip => (
        <li key={trip.id}>
          {trip.name} ({trip.destination})
        </li>
      ))}
    </ul>
  );
}

export default TripList;
