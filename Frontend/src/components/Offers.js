import React, { useState, useContext, useEffect } from 'react';
import { AppContext } from '../App';
import axios from 'axios';
import './Offers.css';
import { useNavigate } from 'react-router-dom';

function Offers({ offers = [] }) {
  const [showAllFeatures, setShowAllFeatures] = useState(false);
  const { clientId } = useContext(AppContext);
  const navigate = useNavigate();
  const toggleShowAllFeatures = () => {
    setShowAllFeatures(!showAllFeatures);
  };
  const reactAppHost = process.env.REACT_APP_HOST || 'localhost';
  const reactAppPort = process.env.REACT_APP_PORT || 3000;

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
      const response = await axios.post(`http://${reactAppHost}:${reactAppPort}/api/Trip/ReserveTrip`, reservationDto);
      console.log(response.data);
      navigate('/user-offers'); 
    } catch (error) {
      console.error(`An error occurred while reserving the offer: `, error);
      alert("Ktoś właśnie zgarnął Ci ofertę sprzed nosa!");
    }
  };

  useEffect(() => {
    console.log("Offers component re-rendered with offers:", offers);
  }, [offers]);

  return (
    <div className="offers" style={{ maxHeight: '700px', overflowY: 'scroll' }}>
      {offers.map((offer, index) => {
        const features = offer.features.split('|');
        const visibleFeatures = showAllFeatures ? features : features.slice(0, 1);

        const isReserved = offer.isReserved;
        console.log(`Rendering offer ${offer.id}, isReserved: ${isReserved}`);
        console.log(`Type of offer.id: ${typeof offer.id}`);

        return (
          <div key={index} className={`offer-card ${isReserved ? 'reserved' : ''}`}>
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
            {!isReserved && (
              <>
                <button onClick={toggleShowAllFeatures}>
                  {showAllFeatures ? 'Pokaż mniej' : 'Pokaż więcej'}
                </button>
                {offer.status == "Dostępna" && clientId && (
                  <button onClick={() => handleReserve(offer)}>Rezerwuj</button>
                )}
              </>
            )}
            <p>Status: {offer.status}</p>
            {isReserved && <div className="reserved-message">Ta oferta właśnie została zarezerwowana</div>}
          </div>
        );
      })}
    </div>
  );
}

export default Offers;
