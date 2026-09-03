import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getTaskById, createTask, updateTask } from '../api/tasks';
import { getCategories } from '../api/categories';
import { getUsers } from '../api/users';

export default function TaskForm() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [categories, setCategories] = useState([]);
  const [users, setUsers] = useState([]);
  const [form, setForm] = useState({
    title: '', description: '', status: 'Pending', priority: 'Medium',
    categoryId: '', assignedToUserId: '', dueDate: ''
  });

  useEffect(() => {
    getCategories().then((res) => setCategories(res.data));
    getUsers().then((res) => setUsers(res.data));
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
  }, [id]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    const payload = {
      ...form,
      categoryId: Number(form.categoryId),
      assignedToUserId: Number(form.assignedToUserId),
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
    <div style={{ maxWidth: 500 }}>
      <h2>{isEdit ? 'Edit Task' : 'New Task'}</h2>
      <form onSubmit={handleSubmit}>
        <input name="title" placeholder="Title" value={form.title} onChange={handleChange} required />
        <br /><br />
        <textarea name="description" placeholder="Description" value={form.description} onChange={handleChange} />
        <br /><br />
        {isEdit && (
          <select name="status" value={form.status} onChange={handleChange}>
            <option value="Pending">Pending</option>
            <option value="InProgress">In Progress</option>
            <option value="Completed">Completed</option>
          </select>
        )}
        <br /><br />
        <select name="priority" value={form.priority} onChange={handleChange}>
          <option value="Low">Low</option>
          <option value="Medium">Medium</option>
          <option value="High">High</option>
        </select>
        <br /><br />
        <select name="categoryId" value={form.categoryId} onChange={handleChange} required>
          <option value="">Select Category</option>
          {categories.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
        </select>
        <br /><br />
        <select name="assignedToUserId" value={form.assignedToUserId} onChange={handleChange} required>
          <option value="">Assign To</option>
          {users.map((u) => <option key={u.id} value={u.id}>{u.fullName}</option>)}
        </select>
        <br /><br />
        <input type="date" name="dueDate" value={form.dueDate} onChange={handleChange} />
        <br /><br />
        <button type="submit">Save</button>
      </form>
    </div>
  );
}