import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Offer from './Offers';
import './TripList.css';

function TripList({ reservedOffer }) {
  const [trips, setTrips] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const result = await axios('api/Trip/GetAllTrips');
      setTrips(result.data);
    };

    fetchData();
  }, []);

  return (
    <div className="trip-list">
      {trips.map(trip => (
        <Offer
          key={trip.id}
          id={trip.id}
          title={trip.name}
          destination={trip.destination}
          reservedOfferId={reservedOffer}
        />
      ))}
    </div>
  );
}

export default TripList;
