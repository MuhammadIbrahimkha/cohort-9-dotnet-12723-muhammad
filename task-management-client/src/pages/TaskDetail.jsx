import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getTaskById } from '../api/tasks';

const fields = [
  { key: 'description', label: 'Description', fallback: '-' },
  { key: 'status', label: 'Status' },
  { key: 'priority', label: 'Priority' },
  { key: 'categoryName', label: 'Category', fallback: '-' },
  { key: 'assignedToUserName', label: 'Assigned to', fallback: '-' },
];

export default function TaskDetail() {
  const { id } = useParams();
  const [task, setTask] = useState(null);

  useEffect(() => {
    getTaskById(id).then((res) => setTask(res.data));
  }, [id]);

  if (!task) return <p className="text-sm text-muted">Loading…</p>;

  return (
    <div className="max-w-lg">
      <Link to="/tasks" className="text-sm text-muted hover:text-ink">
        ← Back to tasks
      </Link>

      <h1 className="mt-3 font-display text-2xl font-semibold">{task.title}</h1>

      <dl className="mt-6 divide-y divide-line border-y border-line">
        {fields.map((f) => (
          <div key={f.key} className="flex justify-between py-3">
            <dt className="text-sm text-muted">{f.label}</dt>
            <dd className="text-sm text-ink">{task[f.key] || f.fallback}</dd>
          </div>
        ))}
        <div className="flex justify-between py-3">
          <dt className="text-sm text-muted">Due date</dt>
          <dd className="text-sm text-ink">
            {task.dueDate ? new Date(task.dueDate).toLocaleDateString() : '-'}
          </dd>
        </div>
      </dl>

      <Link
        to={`/tasks/${task.id}/edit`}
        className="mt-6 inline-block rounded bg-pine px-4 py-2 text-sm font-medium text-white hover:bg-pine-dark"
      >
        Edit task
      </Link>
    </div>
  );
}