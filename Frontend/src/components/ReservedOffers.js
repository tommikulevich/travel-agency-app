import React, { useEffect, useState, useContext, useCallback } from 'react'; 
import axios from 'axios';
import { useLocation } from 'react-router-dom';
import { AppContext } from '../App';

const ReservedOffers = () => {
  const [offers, setOffers] = useState([]);
  const { clientId } = useContext(AppContext)
  const location = useLocation();

  const reactAppHost = process.env.REACT_APP_HOST || 'localhost';
  const reactAppPort = process.env.REACT_APP_PORT || 3000;

  const fetchData = useCallback(async () => {
    try {
      const result = await axios.get(`http://${reactAppHost}:${reactAppPort}/api/Trip/GetTripsByUserId`, {
        params: { ClientId: clientId }
      });
      setOffers(result.data);
    } catch (error) {
      console.error('Błąd podczas pobierania ofert:', error);
    }
  }, [clientId, reactAppHost, reactAppPort]);

  useEffect(() => {
    if (clientId) {
      fetchData();
    }
  }, [clientId, fetchData]); 

  useEffect(() => {
    fetchData();
  }, [location.key, fetchData]); 

  const handleBuy = async (offerId, offerPrice) => {
    try {
      const response = await axios.post(`http://${reactAppHost}:${reactAppPort}/api/Payment/Payment`, null, {
        params: { OfferId: offerId, Price: offerPrice }
      });
      if (response.data.result) {
        alert("Płatność udana");
      } else {
        alert("Płatność odrzucona");
      }
      fetchData();
    } catch (error) {
      console.error(`Błąd podczas zakupu oferty ${offerId}: `, error); 
    }
  };

  const [buttonClicked, setButtonClicked] = useState(false);

  const handleClick = (id, price) => {
    handleBuy(id, price);
    setButtonClicked(true);
  };

  return (
    <div className="reserved-offers" style={{ maxHeight: '700px', overflowY: 'scroll' }}>
      <h2>Twoje zarezerwowane oferty</h2>
      {offers.map((offer, index) => (
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
          {offer.status === 'Oczekiwanie na płatność' && !buttonClicked && (
            <button onClick={() => handleClick(offer.id, offer.price)}>Zapłać</button>
          )}
          <p>Status: {offer.status}</p>
        </div>
      ))}
    </div>
  );
};

export default ReservedOffers;