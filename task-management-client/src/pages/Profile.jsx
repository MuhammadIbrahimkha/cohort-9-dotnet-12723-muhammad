import { useEffect, useState } from 'react';
import { getMe } from '../api/users';

const fields = [
  { key: 'fullName', label: 'Name' },
  { key: 'email', label: 'Email' },
  { key: 'role', label: 'Role' },
];

export default function Profile() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    getMe().then((res) => setUser(res.data));
  }, []);

  if (!user) return <p className="text-sm text-muted">Loading…</p>;

  return (
    <div className="max-w-lg">
      <h1 className="font-display text-2xl font-semibold">Profile</h1>

      <dl className="mt-6 divide-y divide-line border-y border-line">
        {fields.map((f) => (
          <div key={f.key} className="flex justify-between py-3">
            <dt className="text-sm text-muted">{f.label}</dt>
            <dd className="text-sm text-ink">{user[f.key]}</dd>
          </div>
        ))}
      </dl>
    </div>
  );
}