import React, { useState, useContext, useEffect } from 'react';
import { AppContext } from '../App';
import axios from 'axios';
import './Offers.css';
import { useNavigate } from 'react-router-dom'; // Importuj useNavigate

function Offers({ offers }) {
  const [showAllFeatures, setShowAllFeatures] = useState(false);
  const { clientId } = useContext(AppContext);
  const navigate = useNavigate();
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
  
    try {
      const response = await axios.post('http://localhost:8080/api/Trip/ReserveTrip', reservationDto);
      console.log(response.data);
      navigate('/user-offers'); // Przekieruj użytkownika do '/user-offers'
    } catch (error) {
      console.error(`An error occurred while reserving the offer: `, error);
      alert("Ktoś właśnie zgarnął Ci ofertę sprzed nosa!");
      // Here you can add code to handle the error, like showing a message to the user
    }
  };

  return (
    <div className="offers" style={{ maxHeight: '700px', overflowY: 'scroll' }}>
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
            <p>Rodzaj pokoju: {offer.roomType}</p>
            <p>Liczba dorosłych: {offer.numOfAdults}
            &nbsp;&nbsp;&nbsp;nastolatków: {offer.numOfKidsTo18}
            &nbsp;&nbsp;&nbsp; młodzieży: {offer.numOfKidsTo10}
            &nbsp;&nbsp;&nbsp; dzieci: {offer.numOfKidsTo3}</p>
            <p>Cena: {offer.price}</p>
            <p>Typ posiłków: {offer.mealsType}</p>
            <p>Zniżka: {offer.discountPercents * 100}%</p>
            <div>
              Cechy: {visibleFeatures.join(', ')}
            </div>
            <button onClick={toggleShowAllFeatures}>
              {showAllFeatures ? 'Pokaż mniej' : 'Pokaż więcej'}
            </button>
            {offer.status!=="Zarezerwowana" && clientId && <button onClick={() => handleReserve(offer)}>Rezerwuj</button>} {/* Dodaj warunek do renderowania przycisku */}
            <p>Status: {offer.status}</p>
          </div>
        );
      })}
    </div>
  );
}

export default Offers;