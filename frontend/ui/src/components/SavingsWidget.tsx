import { formatCurrency } from '../utils/formatters';
import type { BudgetSavings } from '../types/analytics';

interface Props {
  data: BudgetSavings;
}

export const SavingsWidget = ({ data }: Props) => {
  const isNegative = data.totalSavings < 0;

  return (
    <div className="savings-widget">
      <h2>Економія бюджету</h2>
      <div className={`savings-amount ${isNegative ? 'negative' : 'positive'}`}>
        {formatCurrency(data.totalSavings)}
      </div>
      <div className="savings-details">
        <div>
          <span>Бюджет:</span>
          <strong>{formatCurrency(data.totalBudget)}</strong>
        </div>
        <div>
          <span>Контракти:</span>
          <strong>{formatCurrency(data.totalContracts)}</strong>
        </div>
      </div>
    </div>
  );
};