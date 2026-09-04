import { Link, Outlet, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const navLinks = [
  { to: '/dashboard', label: 'Dashboard' },
  { to: '/tasks', label: 'Tasks' },
  { to: '/profile', label: 'Profile' },
];

export default function Layout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="min-h-screen bg-paper font-sans text-ink">
      <nav className="flex flex-wrap items-center gap-x-6 gap-y-3 border-b border-line px-4 py-4 sm:px-8">
        <span className="font-display text-lg font-semibold tracking-tight">
          Task Management
        </span>
        <div className="flex gap-5 sm:gap-6">
          {navLinks.map((link) => {
            const active = location.pathname.startsWith(link.to);
            return (
              <Link
                key={link.to}
                to={link.to}
                className={
                  active
                    ? 'text-sm font-medium text-pine'
                    : 'text-sm font-medium text-muted hover:text-ink'
                }
              >
                {link.label}
              </Link>
            );
          })}
        </div>
        <div className="ml-auto flex items-center gap-3 sm:gap-4">
          <span className="hidden text-sm text-muted sm:inline">{user?.fullName}</span>
          <button
            onClick={handleLogout}
            className="rounded border border-line px-3 py-1.5 text-sm text-ink hover:border-rust hover:text-rust"
          >
            Logout
          </button>
        </div>
      </nav>
      <main className="mx-auto max-w-5xl px-4 py-8 sm:px-8 sm:py-10">
        <Outlet />
      </main>
    </div>
  );
}