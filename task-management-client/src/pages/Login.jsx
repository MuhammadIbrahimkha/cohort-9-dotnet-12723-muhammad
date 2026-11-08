import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { loginUser } from '../api/auth';
import { useAuth } from '../context/AuthContext';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const res = await loginUser({ email, password });
      login(res.data);
      navigate('/dashboard');
    } catch (err) {
      setError(err.response?.data?.message || 'Login failed');
    }
  };

  return (
<div className="flex min-h-screen items-center justify-center bg-paper px-4 font-sans text-ink">      <div className="w-full max-w-sm">
        <h1 className="font-display text-2xl font-semibold">Log in</h1>
        <p className="mt-1 text-sm text-muted">
          Sign in to view and manage your tasks.
        </p>

        {error && (
          <p className="mt-4 border-l-2 border-rust bg-rust/5 px-3 py-2 text-sm text-rust">
            {error}
          </p>
        )}

        <form onSubmit={handleSubmit} className="mt-6 flex flex-col gap-4">
          <div>
            <label className="mb-1 block text-sm font-medium text-muted">Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="w-full rounded border border-line bg-white px-3 py-2 text-sm outline-none focus:border-pine"
            />
          </div>
          <div>
            <label className="mb-1 block text-sm font-medium text-muted">Password</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="w-full rounded border border-line bg-white px-3 py-2 text-sm outline-none focus:border-pine"
            />
          </div>
          <button
            type="submit"
            className="mt-2 rounded bg-pine px-4 py-2 text-sm font-medium text-white hover:bg-pine-dark"
          >
            Log in
          </button>
        </form>

        <p className="mt-6 text-sm text-muted">
          No account?{' '}
          <Link to="/signup" className="text-pine hover:underline">
            Sign up
          </Link>
        </p>
      </div>
    </div>
  );
}