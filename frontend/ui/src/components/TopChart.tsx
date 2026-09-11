import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid,
  Tooltip, ResponsiveContainer
} from 'recharts';
import { formatAmount, shortName } from '../utils/formatters';

interface ChartItem {
  name: string;
  totalAmount: number;
  count: number;
}

interface Props {
  title: string;
  data: ChartItem[];
  countLabel: string;
  color: string;
}

export const TopChart = ({ title, data, countLabel, color }: Props) => {
  const chartData = data.map(item => ({
    name: shortName(item.name),
    fullName: item.name,
    amount: item.totalAmount,
    count: item.count,
  }));

  return (
    <div className="chart-container">
      <h2>{title}</h2>
      <ResponsiveContainer width="100%" height={300}>
        <BarChart data={chartData} layout="vertical">
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis type="number" tickFormatter={formatAmount} />
          <YAxis type="category" dataKey="name" width={200} tick={{ fontSize: 11 }} />
          <Tooltip
            formatter={(value) => {
                  if (typeof value !== 'number') return ['—', 'Сума'];
                  return [formatAmount(value), 'Сума'];
            }}
            labelFormatter={(label) => chartData.find(d => d.name === label)?.fullName ?? label}
          />
          <Bar dataKey="amount" fill={color} />
        </BarChart>
      </ResponsiveContainer>

      <table className="data-table">
        <thead>
          <tr>
            <th>Назва</th>
            <th>Сума</th>
            <th>{countLabel}</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item, i) => (
            <tr key={i}>
              <td>{item.name}</td>
              <td>{formatAmount(item.totalAmount)}</td>
              <td>{item.count}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};