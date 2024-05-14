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
        const visibleFeatures = showAllFeatures ? features : features.slice(0, 1);

        return (
          <div key={index} className="offer-card">
            <h2>{offer.name}</h2>
            <p>Kraj: {offer.country}</p>
            <p>Miasto: {offer.city}</p>
            <p>Miejsce wyjazdu: {offer.departurePlace}</p>
            <p>Miejsce przyjazdu: {offer.arrivalPlace}</p>
            <p>Data wyjazdu: {offer.departureDate}</p>
            <p>Data powrotu: {offer.returnDate}</p>
            <p>Typ transportu: {offer.transportType}</p>
            <p>Cena: {offer.price}</p>
            <p>Typ posiłków: {offer.mealsType}</p>
            <p>Zniżka: {offer.discountPercents * 100}%</p>
            <div>
              Cechy: {visibleFeatures.join(', ')}
            </div>
            <button onClick={toggleShowAllFeatures}>
              {showAllFeatures ? 'Pokaż mniej' : 'Pokaż więcej'}
            </button>
            <button onClick={() => handleReserve(offer)}>Rezerwuj</button>
            <p>Status: {offer.status}</p>
          </div>
        );
      })}
    </div>
  );
}

export default Offers;