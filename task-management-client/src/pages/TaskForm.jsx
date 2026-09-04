import { useEffect, useState } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { getTaskById, createTask, updateTask } from '../api/tasks';
import { getCategories } from '../api/categories';
import { getUsers } from '../api/users';
import { useAuth } from '../context/AuthContext';

const inputClass =
  'w-full rounded border border-line bg-white px-3 py-2 text-sm outline-none focus:border-pine';
const labelClass = 'mb-1 block text-sm font-medium text-muted';

export default function TaskForm() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';

  const [categories, setCategories] = useState([]);
  const [users, setUsers] = useState([]);
  const [form, setForm] = useState(() => ({
    title: '', description: '', status: 'Pending', priority: 'Medium',
    categoryId: '', assignedToUserId: isAdmin ? '' : (user?.userId ?? ''), dueDate: ''
  }));

  useEffect(() => {
    getCategories().then((res) => setCategories(res.data));
    if (isAdmin) {
      getUsers().then((res) => setUsers(res.data));
    }
    if (isEdit) {
      getTaskById(id).then((res) => {
        const t = res.data;
        setForm({
          title: t.title, description: t.description || '', status: t.status,
          priority: t.priority, categoryId: t.categoryId, assignedToUserId: t.assignedToUserId,
          dueDate: t.dueDate ? t.dueDate.split('T')[0] : ''
        });
      });
    }
  }, [id, isAdmin]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    const payload = {
      ...form,
      categoryId: Number(form.categoryId),
      assignedToUserId: isAdmin ? Number(form.assignedToUserId) : user.userId,
      dueDate: form.dueDate || null
    };
    if (isEdit) {
      await updateTask(id, payload);
    } else {
      await createTask(payload);
    }
    navigate('/tasks');
  };

  return (
    <div className="max-w-lg">
      <h1 className="font-display text-2xl font-semibold">
        {isEdit ? 'Edit task' : 'New task'}
      </h1>

      <form onSubmit={handleSubmit} className="mt-6 flex flex-col gap-4">
        <div>
          <label className={labelClass}>Title</label>
          <input
            name="title"
            value={form.title}
            onChange={handleChange}
            required
            className={inputClass}
          />
        </div>

        <div>
          <label className={labelClass}>Description</label>
          <textarea
            name="description"
            rows={3}
            value={form.description}
            onChange={handleChange}
            className={inputClass}
          />
        </div>

        {isEdit && (
          <div>
            <label className={labelClass}>Status</label>
            <select name="status" value={form.status} onChange={handleChange} className={inputClass}>
              <option value="Pending">Pending</option>
              <option value="InProgress">In Progress</option>
              <option value="Completed">Completed</option>
            </select>
          </div>
        )}

        <div>
          <label className={labelClass}>Priority</label>
          <select name="priority" value={form.priority} onChange={handleChange} className={inputClass}>
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
          </select>
        </div>

        <div>
          <label className={labelClass}>Category</label>
          <select name="categoryId" value={form.categoryId} onChange={handleChange} required className={inputClass}>
            <option value="">Select category</option>
            {categories.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
          </select>
        </div>

        {isAdmin && (
          <div>
            <label className={labelClass}>Assign to</label>
            <select
              name="assignedToUserId"
              value={form.assignedToUserId}
              onChange={handleChange}
              required
              className={inputClass}
            >
              <option value="">Select person</option>
              {users.map((u) => <option key={u.id} value={u.id}>{u.fullName}</option>)}
            </select>
          </div>
        )}

        <div>
          <label className={labelClass}>Due date</label>
          <input
            type="date"
            name="dueDate"
            value={form.dueDate}
            onChange={handleChange}
            className={inputClass}
          />
        </div>

        <div className="mt-2 flex gap-3">
          <button
            type="submit"
            className="rounded bg-pine px-4 py-2 text-sm font-medium text-white hover:bg-pine-dark"
          >
            Save
          </button>
          <Link
            to="/tasks"
            className="rounded border border-line px-4 py-2 text-sm text-muted hover:text-ink"
          >
            Cancel
          </Link>
        </div>
      </form>
    </div>
  );
}