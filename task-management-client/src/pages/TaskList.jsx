import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getTasks, deleteTask } from '../api/tasks';

export default function TaskList() {
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
      <div style={{ display: 'flex', justifyContent: 'space-between' }}>
        <h2>Tasks</h2>
        <Link to="/tasks/new"><button>New Task</button></Link>
      </div>
      <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)}>
        <option value="">All Statuses</option>
        <option value="Pending">Pending</option>
        <option value="InProgress">In Progress</option>
        <option value="Completed">Completed</option>
      </select>
      <table width="100%" style={{ marginTop: 16 }}>
        <thead>
          <tr><th>Title</th><th>Status</th><th>Priority</th><th>Due Date</th><th></th></tr>
        </thead>
        <tbody>
          {tasks.map((t) => (
            <tr key={t.id}>
              <td><Link to={`/tasks/${t.id}`}>{t.title}</Link></td>
              <td>{t.status}</td>
              <td>{t.priority}</td>
              <td>{t.dueDate ? new Date(t.dueDate).toLocaleDateString() : '-'}</td>
              <td>
                <Link to={`/tasks/${t.id}/edit`}>Edit</Link>{' '}
                <button onClick={() => handleDelete(t.id)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}