import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getTasks, deleteTask } from '../api/tasks';
import { useAuth } from '../context/AuthContext';

const priorityColor = {
  High: 'bg-rust',
  Medium: 'bg-amber',
  Low: 'bg-pine',
};

const statusStyle = {
  Pending: 'text-muted',
  InProgress: 'text-amber',
  Completed: 'text-pine',
};

export default function TaskList() {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';
  const [tasks, setTasks] = useState([]);
  const [statusFilter, setStatusFilter] = useState('');

  const fetchTasks = () => {
    getTasks(statusFilter ? { status: statusFilter } : {}).then((res) => setTasks(res.data));
  };

  useEffect(() => {
    fetchTasks();
  }, [statusFilter]);

  const handleDelete = async (id) => {
    await deleteTask(id);
    fetchTasks();
  };

  return (
    <div>
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="font-display text-2xl font-semibold">Tasks</h1>
          <p className="mt-1 text-sm text-muted">
            {isAdmin ? 'All tasks across the team.' : 'Tasks assigned to you.'}
          </p>
        </div>
        <Link
          to="/tasks/new"
          className="inline-block w-fit rounded bg-pine px-4 py-2 text-sm font-medium text-white hover:bg-pine-dark"
        >
          New Task
        </Link>
      </div>

      <select
        value={statusFilter}
        onChange={(e) => setStatusFilter(e.target.value)}
        className="mt-6 rounded border border-line bg-white px-3 py-1.5 text-sm outline-none focus:border-pine"
      >
        <option value="">All statuses</option>
        <option value="Pending">Pending</option>
        <option value="InProgress">In progress</option>
        <option value="Completed">Completed</option>
      </select>

      <div className="mt-4 border-t border-line">
        {tasks.length === 0 && (
          <p className="py-8 text-sm text-muted">No tasks to show.</p>
        )}
        {tasks.map((t) => (
          <div
            key={t.id}
            className="flex flex-col gap-3 border-b border-line py-3 sm:flex-row sm:items-center sm:gap-4"
          >
            {/* icon + title */}
            <div className="flex min-w-0 items-center gap-3 sm:flex-1">
              <span className={`h-8 w-1 shrink-0 rounded-full ${priorityColor[t.priority] || 'bg-line'}`} />
              <Link
                to={`/tasks/${t.id}`}
                className="min-w-0 truncate text-sm font-medium text-ink hover:text-pine"
              >
                {t.title}
              </Link>
            </div>

            {/* status / priority / date / actions */}
            <div className="flex flex-wrap items-center gap-x-4 gap-y-1 pl-4 text-sm sm:flex-nowrap sm:pl-0">
              <span className={`sm:w-24 sm:shrink-0 ${statusStyle[t.status] || 'text-muted'}`}>
                {t.status}
              </span>
              <span className="text-muted sm:w-16 sm:shrink-0">{t.priority}</span>
              <span className="text-muted sm:w-24 sm:shrink-0">
                {t.dueDate ? new Date(t.dueDate).toLocaleDateString() : '-'}
              </span>
              <div className="flex shrink-0 gap-3">
                <Link to={`/tasks/${t.id}/edit`} className="text-pine hover:underline">
                  Edit
                </Link>
                {isAdmin && (
                  <button
                    onClick={() => handleDelete(t.id)}
                    className="text-rust hover:underline"
                  >
                    Delete
                  </button>
                )}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}