import { useEffect, useState } from 'react';
import { getDashboardSummary } from '../api/dashboard';

const stats = [
  { key: 'pendingCount', label: 'Pending' },
  { key: 'inProgressCount', label: 'In progress' },
  { key: 'completedCount', label: 'Completed' },
];

export default function Dashboard() {
  const [summary, setSummary] = useState(null);

  useEffect(() => {
    getDashboardSummary().then((res) => setSummary(res.data));
  }, []);

  return (
    <div>
      <h1 className="font-display text-2xl font-semibold">Dashboard</h1>
      <p className="mt-1 text-sm text-muted">An overview of where things stand.</p>

      {!summary ? (
        <p className="mt-8 text-sm text-muted">Loading…</p>
      ) : (
        <div className="mt-8 flex flex-col divide-y divide-line border-y border-line sm:flex-row sm:divide-x sm:divide-y-0">
          {stats.map((s) => (
            <div key={s.key} className="flex-1 py-4 sm:px-6 sm:py-5 sm:first:pl-0">
              <p className="font-display text-4xl font-semibold">{summary[s.key]}</p>
              <p className="mt-1 text-sm text-muted">{s.label}</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}