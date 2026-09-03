import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getTaskById } from '../api/tasks';

export default function TaskDetail() {
  const { id } = useParams();
  const [task, setTask] = useState(null);

  useEffect(() => {
    getTaskById(id).then((res) => setTask(res.data));
  }, [id]);

  if (!task) return <p>Loading...</p>;

  return (
    <div>
      <h2>{task.title}</h2>
      <p><b>Description:</b> {task.description || '-'}</p>
      <p><b>Status:</b> {task.status}</p>
      <p><b>Priority:</b> {task.priority}</p>
      <p><b>Category:</b> {task.categoryName || '-'}</p>
      <p><b>Assigned To:</b> {task.assignedToUserName || '-'}</p>
      <p><b>Due Date:</b> {task.dueDate ? new Date(task.dueDate).toLocaleDateString() : '-'}</p>
      <Link to={`/tasks/${task.id}/edit`}>Edit</Link>
      {' | '}
      <Link to="/tasks">Back to list</Link>
    </div>
  );
}