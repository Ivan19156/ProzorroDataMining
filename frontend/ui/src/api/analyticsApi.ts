import axios from 'axios';
import { API_BASE, API_ENDPOINTS } from '../constants/api';
import type { BudgetSavings, TopBuyer, TopSupplier } from '../types/analytics';

const api = axios.create({
  baseURL: API_BASE,
});

export const analyticsApi = {
  getBudgetSavings: () =>
    api.get<BudgetSavings>(API_ENDPOINTS.savings).then(r => r.data),

  getTopBuyers: (top = 5) =>
    api.get<TopBuyer[]>(`${API_ENDPOINTS.topBuyers}?top=${top}`).then(r => r.data),

  getTopSuppliers: (top = 5) =>
    api.get<TopSupplier[]>(`${API_ENDPOINTS.topSuppliers}?top=${top}`).then(r => r.data),

  runIngestion: () =>
    api.post(API_ENDPOINTS.ingestionRun).then(r => r.data),
};