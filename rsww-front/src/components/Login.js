import React, { useState } from 'react';
import axios from 'axios';

function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = async event => {
    event.preventDefault();
    // Tutaj możesz wysłać dane do swojego backendu
    try {
      const response = await axios.post('/api/auth/login', { username, password });
      console.log(response.data);
    } catch (error) {
      console.error(error);
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
