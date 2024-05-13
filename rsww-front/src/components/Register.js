import React, { useState } from 'react';
import axios from 'axios';

function Register() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const handleSubmit = async event => {
    event.preventDefault();
    if (password !== confirmPassword) {
      alert('Hasła nie są takie same!');
      return;
    }
    // Tutaj możesz wysłać dane do swojego backendu
    try {
      const response = await axios.post('/api/auth/register', { username, password });
      console.log(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="register">
      <h2>Rejestracja</h2>
      <form onSubmit={handleSubmit}>
        <label>
          Nazwa użytkownika:
          <input type="text" value={username} onChange={e => setUsername(e.target.value)} />
        </label>
        <label>
          Hasło:
          <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
        </label>
        <label>
          Powtórz hasło:
          <input type="password" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} />
        </label>
        <input type="submit" value="Zarejestruj się" />
      </form>
    </div>
  );
}

export default Register;
