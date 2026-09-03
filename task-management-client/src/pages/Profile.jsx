import { useEffect, useState } from 'react';
import { getMe } from '../api/users';

export default function Profile() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    getMe().then((res) => setUser(res.data));
  }, []);

  if (!user) return <p>Loading...</p>;

  return (
    <div>
      <h2>Profile</h2>
      <p><b>Name:</b> {user.fullName}</p>
      <p><b>Email:</b> {user.email}</p>
      <p><b>Role:</b> {user.role}</p>
    </div>
  );
}