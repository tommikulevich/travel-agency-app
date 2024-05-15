import React, { useEffect, useState, useContext, useCallback } from 'react'; // Importuj useCallback
import axios from 'axios';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { AppContext } from '../App';

function ReservedOffers() {
  const [offers, setOffers] = useState([]);
  const { clientId } = useContext(AppContext);
  const navigate = useNavigate(); // Przenieś useNavigate() tutaj
  const location = useLocation();

  const fetchData = useCallback(async () => { // Użyj useCallback
    console.log('UserID_ostronieofert:', clientId);
    const result = await axios.get(`http://localhost:8080/api/Trip/GetTripsByUserId?ClientId=${clientId}`);
    setOffers(result.data);
  }, [clientId]); // Dodaj clientId do tablicy zależności useCallback

  useEffect(() => {
    if (clientId) {
      fetchData();
    }
  }, [clientId, fetchData]); // fetchData jest teraz taka sama przy każdym renderowaniu, o ile nie zmieni się clientId

  useEffect(() => {
    fetchData(); // Odśwież dane, gdy klucz lokalizacji się zmienia
  }, [location.key, fetchData]); // Dodaj location.key do tablicy zależności



  const handleBuy = async (offerId, offerPrice) => {
    try {
      const response = await axios.post(`http://localhost:8080/api/Payment/Payment?OfferId=${offerId}&&Price=${offerPrice}`);  // Zmieniamy URL na odpowiedni /Payment?OfferId=d5e60983-98e5-44a8-b9cb-9379927401b8&Price=1245
      console.log(offerId);
      console.log(response); // Wyświetlamy odpowiedź w konsoli

      if (response.data.result) {
        alert("Płatność udana");
      } else {
        alert("Płatność odrzucona");
      }
      fetchData();
    } catch (error) {
      console.error(`Błąd podczas zakupu oferty ${offerId}: `, error); // Wyświetlamy błąd w konsoli
    }
  };

  return (
    <div className="reserved-offers" style={{ maxHeight: '700px', overflowY: 'scroll' }}>
      <h2>Twoje zarezerwowane oferty</h2>
      <p>ID klienta: {clientId}</p> {/* Dodaj tę linię */}
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
          {offer.status == 'Oczekiwanie na płatność' && <button onClick={() => handleBuy(offer.id, offer.price)}>Zapłać</button>} {/* Dodaj warunek do renderowania przycisku */}
        <p>Status: {offer.status}</p>
        </div>
      ))}
    </div>
  );
}

export default ReservedOffers;
