export const API_BASE = import.meta.env.VITE_API_BASE_URL ?? '/api';

export const API_ENDPOINTS = {
  savings: '/analytics/savings',
  topBuyers: '/analytics/top-buyers',
  topSuppliers: '/analytics/top-suppliers',
  ingestionRun: '/ingestion/run',
} as const;