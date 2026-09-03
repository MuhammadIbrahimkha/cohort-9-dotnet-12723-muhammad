import { Link, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Layout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div>
      <nav style={{ padding: 16, borderBottom: '1px solid #ccc', display: 'flex', gap: 16 }}>
        <Link to="/dashboard">Dashboard</Link>
        <Link to="/tasks">Tasks</Link>
        <Link to="/profile">Profile</Link>
        <span style={{ marginLeft: 'auto' }}>{user?.fullName}</span>
        <button onClick={handleLogout}>Logout</button>
      </nav>
      <div style={{ padding: 24 }}>
        <Outlet />
      </div>
    </div>
  );
}