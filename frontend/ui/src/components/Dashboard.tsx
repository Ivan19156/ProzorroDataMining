// src/components/Dashboard.tsx
import { useState } from 'react';
import { analyticsApi } from '../api/analyticsApi';
import { useAnalytics } from '../hooks/useAnalytics';
import { SavingsWidget } from './SavingsWidget';
import { TopBuyersChart } from './TopBuyersChart';
import { TopSuppliersChart } from './TopSuppliersChart';

export const Dashboard = () => {
  const { savings, buyers, suppliers, loading, error } = useAnalytics();
  const [ingesting, setIngesting] = useState(false);
  const [ingestionError, setIngestionError] = useState<string | null>(null);

  const handleIngestion = async () => {
    try {
      setIngesting(true);
      setIngestionError(null);
      await analyticsApi.runIngestion();
      alert('Імпорт запущено у фоні. Дані оновляться автоматично через кілька хвилин.');
    } catch {
      setIngestionError('Помилка запуску імпорту. Спробуйте ще раз.');
    } finally {
      setIngesting(false);
    }
  };

  if (loading) return <div className="loading">Завантаження...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div className="dashboard">
      <header className="dashboard-header">
        <h1>Аналітика Prozorro — CPV 09310000-5</h1>
        <div className="header-actions">
          <button
            className="refresh-btn"
            onClick={handleIngestion}
            disabled={ingesting}
          >
            {ingesting ? 'Імпортується...' : 'Оновити дані'}
          </button>
          {ingestionError && (
            <span className="ingestion-error">{ingestionError}</span>
          )}
        </div>
      </header>

      {savings && <SavingsWidget data={savings} />}

      <div className="charts-grid">
        <TopBuyersChart data={buyers} />
        <TopSuppliersChart data={suppliers} />
      </div>
    </div>
  );
};