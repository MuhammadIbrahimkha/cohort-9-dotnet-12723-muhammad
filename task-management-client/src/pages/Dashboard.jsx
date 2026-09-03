import { useEffect, useState } from 'react';
import { getDashboardSummary } from '../api/dashboard';

export default function Dashboard() {
  const [summary, setSummary] = useState(null);

  useEffect(() => {
    getDashboardSummary().then((res) => setSummary(res.data));
  }, []);

  if (!summary) return <p>Loading...</p>;

  return (
    <div>
      <h2>Dashboard</h2>
      <div style={{ display: 'flex', gap: 24 }}>
        <div style={{ border: '1px solid #ccc', padding: 16 }}>
          <h3>Pending</h3>
          <p>{summary.pendingCount}</p>
        </div>
        <div style={{ border: '1px solid #ccc', padding: 16 }}>
          <h3>In Progress</h3>
          <p>{summary.inProgressCount}</p>
        </div>
        <div style={{ border: '1px solid #ccc', padding: 16 }}>
          <h3>Completed</h3>
          <p>{summary.completedCount}</p>
        </div>
      </div>
    </div>
  );
}