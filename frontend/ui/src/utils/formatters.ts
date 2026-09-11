export const formatAmount = (value: number): string =>
  `${(value / 1_000_000).toFixed(1)}M ₴`;

export const formatCurrency = (amount: number): string =>
  new Intl.NumberFormat('uk-UA', {
    style: 'currency',
    currency: 'UAH',
    maximumFractionDigits: 0,
  }).format(amount);

export const shortName = (name: string, maxLength = 30): string =>
  name.length > maxLength ? name.substring(0, maxLength) + '...' : name;