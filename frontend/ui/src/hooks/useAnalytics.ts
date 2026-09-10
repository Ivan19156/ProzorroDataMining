// src/hooks/useAnalytics.ts
import { useState, useEffect, useCallback } from 'react';
import { analyticsApi } from '../api/analyticsApi';
import type { BudgetSavings, TopBuyer, TopSupplier } from '../types/analytics';

interface AnalyticsState {
  savings: BudgetSavings | null;
  buyers: TopBuyer[];
  suppliers: TopSupplier[];
  loading: boolean;
  error: string | null;
}

export const useAnalytics = () => {
  const [state, setState] = useState<AnalyticsState>({
    savings: null,
    buyers: [],
    suppliers: [],
    loading: true,
    error: null,
  });

  const fetchData = useCallback(async () => {
    try {
      setState(prev => ({ ...prev, loading: true, error: null }));
      const [savings, buyers, suppliers] = await Promise.all([
        analyticsApi.getBudgetSavings(),
        analyticsApi.getTopBuyers(),
        analyticsApi.getTopSuppliers(),
      ]);
      setState({ savings, buyers, suppliers, loading: false, error: null });
    } catch {
      setState(prev => ({
        ...prev,
        loading: false,
        error: 'Помилка завантаження даних',
      }));
    }
  }, []);

  useEffect(() => {
    fetchData();
    const interval = setInterval(fetchData, 120_000);
    return () => clearInterval(interval);
  }, [fetchData]);

  return { ...state, fetchData };
};