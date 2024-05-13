import React from 'react';
import './Offers.css';

function Offers({ offers }) {
  return (
    <div className="offers">
      {offers.map((offer, index) => (
        <div key={index} className="offer-card">
          <h2>{offer.Name}</h2>
          <p>Country: {offer.Country}</p>
          <p>City: {offer.City}</p>
          <p>Departure Place: {offer.DeparturePlace}</p>
          <p>Departure Date: {offer.DepartureDate}</p>
          <p>Return Date: {offer.ReturnDate}</p>
          <p>Transport Type: {offer.TransportType}</p>
          <p>Price: {offer.Price}</p>
          <p>Meals Type: {offer.MealsType}</p>
          <p>Room Type: {offer.RoomType}</p>
          <p>Discount: {offer.DiscountPercents}%</p>
          <p>Number of Nights: {offer.NumOfNights}</p>
          <p>Features: {offer.Features}</p>
          <p>Status: {offer.Status}</p>
        </div>
      ))}
    </div>
  );
}

export default Offers;
