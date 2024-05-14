import React, { useState } from 'react';
import axios from 'axios';
import './Offers.css';

function Offers({ offers, clientId }) {
  const [showAllFeatures, setShowAllFeatures] = useState(false);

  const toggleShowAllFeatures = () => {
    setShowAllFeatures(!showAllFeatures);
  };

  const handleReserve = async (offer) => {
    const reservationDto = {
      OfferId: offer.id,
      ClientId: clientId,
      FlightId: offer.flightId,
      HotelId: offer.hotelId,
      Name: offer.name,
      Country: offer.country,
      City: offer.city,
      DeparturePlace: offer.departurePlace,
      ArrivalPlace: offer.arrivalPlace,
      NumOfAdults: offer.numOfAdults,
      NumOfKidsTo18: offer.numOfKidsTo18,
      NumOfKidsTo10: offer.numOfKidsTo10,
      NumOfKidsTo3: offer.numOfKidsTo3,
      DepartureDate: offer.departureDate,
      ReturnDate: offer.returnDate,
      TransportType: offer.transportType,
      Price: offer.price,
      MealsType: offer.mealsType,
      RoomType: offer.roomType,
      DiscountPercents: offer.discountPercents,
      NumOfNights: offer.numOfNights,
      Features: offer.features,
      Status: offer.status
    };
  
    const response = await axios.post('http://localhost:8080/api/Trip/ReserveTrip', reservationDto);
    console.log(response.data);
  };

  return (
    <div className="offers">
      {offers.map((offer, index) => {
        const features = offer.features.split('|');
        const visibleFeatures = showAllFeatures ? features : features.slice(0, 4);

        return (
          <div key={index} className="offer-card">
            <h2>{offer.name}</h2>
            <p>Country: {offer.country}</p>
            <p>City: {offer.city}</p>
            <p>Departure Place: {offer.departurePlace}</p>
            <p>Arrival Place: {offer.arrivalPlace}</p>
            <p>Departure Date: {offer.departureDate}</p>
            <p>Return Date: {offer.returnDate}</p>
            <p>Transport Type: {offer.transportType}</p>
            <p>Price: {offer.price}</p>
            <p>Meals Type: {offer.mealsType}</p>
            <p>Room Type: {offer.roomType}</p>
            <p>Discount: {offer.discountPercents * 100}%</p>
            <p>Number of Nights: {offer.numOfNights}</p>
            <div>
              Features:
              {visibleFeatures.map((feature, i) => (
                <p key={i}>{feature}</p>
              ))}
            </div>
            <button onClick={toggleShowAllFeatures}>
              {showAllFeatures ? 'Show Less' : 'Show More'}
            </button>
            <button onClick={() => handleReserve(offer)}>Reserve</button>
            <p>Status: {offer.status}</p>
          </div>
        );
      })}
    </div>
  );
}

export default Offers;
