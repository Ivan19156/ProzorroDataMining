import { TopChart } from './TopChart';
import type { TopBuyer } from '../types/analytics';

interface Props {
  data: TopBuyer[];
}

export const TopBuyersChart = ({ data }: Props) => (
  <TopChart
    title="Топ-5 закупівельників"
    data={data.map(item => ({
      name: item.name,
      totalAmount: item.totalAmount,
      count: item.contractCount,
    }))}
    countLabel="Контракти"
    color="#3b82f6"
  />
);