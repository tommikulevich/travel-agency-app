import React, { useState } from 'react';
import axios from 'axios';
import './SearchForm.css';

axios.interceptors.request.use(request => {
  if (request.method === 'get') {
    console.log(request);
  }
  return request;
});

function SearchForm({ onSearch }) {
  const [destination, setDestination] = useState('Grecja'); // Initialize with 'Cypr'
  const day = '2024-06-07';
  const [departureDate, setDepartureDate] = useState(day);
  const [departurePlace, setDeparturePlace] = useState('Gdańsk (GDN)'); // Initialize with 'Gdańsk (GDN)'
  const [numOfAdults, setNumOfAdults] = useState(1);
  const [numOfKidsTo18, setNumOfKidsTo18] = useState(1);
  const [numOfKidsTo10, setNumOfKidsTo10] = useState(0);
  const [numOfKidsTo3, setNumOfKidsTo3] = useState(1);

  const handleSubmit = async event => {
    event.preventDefault();

    const departureYear = new Date(departureDate).getFullYear();
    if (departureYear > 2026) {
      alert('Rok wyjazdu nie może być późniejszy niż 2026');
      return;
    }

    const searchParams = {
      Destination: destination,
      DepartureDate: departureDate,
      DeparturePlace: departurePlace,
      NumOfAdults: numOfAdults || 0,
      NumOfKidsTo18: numOfKidsTo18 || 0,
      NumOfKidsTo10: numOfKidsTo10 || 0,
      NumOfKidsTo3: numOfKidsTo3 || 0,
    };

    onSearch(searchParams);
  };

  return (
    <form onSubmit={handleSubmit}>
      <label>
        Destynacja:
        <select className="largeSelect" value={destination} onChange={e => setDestination(e.target.value)}>
          <option value="Cypr">Cypr</option>
          <option value="Albania">Albania</option>
          <option value="Hiszpania">Hiszpania</option>
          <option value="Wyspy Kanaryjskie">Wyspy Kanaryjskie</option>
          <option value="Egipt">Egipt</option>
          <option value="Bułgaria">Bułgaria</option>
          <option value="Turcja">Turcja</option>
          <option value="Tunezja">Tunezja</option>
          <option value="Grecja">Grecja</option>
        </select>
      </label>
      <label>
        Data wyjazdu:
        <input type="date" value={departureDate} onChange={e => setDepartureDate(e.target.value)} required/>
      </label>
      <label>
        Miejsce wyjazdu:
        <select className="largeSelect" value={departurePlace} onChange={e => setDeparturePlace(e.target.value)}>
          <option value="Gdańsk (GDN)">Gdańsk (GDN)</option>
          <option value="Łódź (LCJ)">Łódź (LCJ)</option>
          <option value="Katowice (KTW)">Katowice (KTW)</option>
          <option value="Bydgoszcz (BZG)">Bydgoszcz (BZG)</option>
          <option value="Kraków (KRK)">Kraków (KRK)</option>
          <option value="Poznań (POZ)">Poznań (POZ)</option>
          <option value="Wrocław (WRO)">Wrocław (WRO)</option>
          <option value="Warszawa-Radom (RDO)">Warszawa-Radom (RDO)</option>
          <option value="Rzeszów (RZE)">Rzeszów (RZE)</option>
          <option value="Lublin (LUZ)">Lublin (LUZ)</option>
          <option value="Warszawa-Chopina (WAW)">Warszawa-Chopina (WAW)</option>
        </select>
      </label>
      <label>
        Liczba dorosłych:
        <input type="number" min="0" max="10" value={numOfAdults} onChange={e => setNumOfAdults(e.target.value ? e.target.value : 0)} onKeyPress={(event) => {if (!/[0-9]/.test(event.key)) event.preventDefault();}} />
      </label>
      <label>
        Liczba dzieci do 18 lat:
        <input type="number" min="0" max="10" value={numOfKidsTo18} onChange={e => setNumOfKidsTo18(e.target.value ? e.target.value : 0)} onKeyPress={(event) => {if (!/[0-9]/.test(event.key)) event.preventDefault();}} />
      </label>
      <label>
        Liczba dzieci do 10 lat:
        <input type="number" min="0" max="10" value={numOfKidsTo10} onChange={e => setNumOfKidsTo10(e.target.value ? e.target.value : 0)} onKeyPress={(event) => {if (!/[0-9]/.test(event.key)) event.preventDefault();}} />
      </label>
      <label>
        Liczba dzieci do 3 lat:
        <input type="number" min="0" max="10" value={numOfKidsTo3} onChange={e => setNumOfKidsTo3(e.target.value ? e.target.value : 0)} onKeyPress={(event) => {if (!/[0-9]/.test(event.key)) event.preventDefault();}} />
      </label>
      <input type="submit" value="Szukaj" />
    </form>
  );
}

export default SearchForm;