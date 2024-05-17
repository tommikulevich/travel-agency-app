import React, { useState, useContext } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom'; 
import { AppContext } from '../App';

function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const { setClientId } = useContext(AppContext);
  const navigate = useNavigate(); 

  const reactAppHost = process.env.REACT_APP_HOST || 'localhost';
  const reactAppPort = process.env.REACT_APP_PORT || 3000;

  const handleSubmit = async event => {
    event.preventDefault();
    try {
      const response = await axios.post(`http://${reactAppHost}:${reactAppPort}/api/Login/Auth?UserName=${username}&Password=${password}`);
      console.log(response.data);

      if (response.data.userId === 0) {
        alert("Zły użytkownik lub hasło");
        navigate('/');
      } else {
        setClientId(response.data.userId);
        navigate('/'); 
      }
    } catch (error) {
      console.log('Error')
    }
  };

  return (
    <div className="login">
      <h2>Logowanie</h2>
      <form onSubmit={handleSubmit}>
        <label>
          Nazwa użytkownika:
          <input type="text" value={username} onChange={e => setUsername(e.target.value)} />
        </label>
        <label>
          Hasło:
          <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
        </label>
        <input type="submit" value="Zaloguj się" />
      </form>
    </div>
  );
}

export default Login;
