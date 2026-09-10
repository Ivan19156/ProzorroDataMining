import { TopChart } from './TopChart';
import type { TopSupplier } from '../types/analytics';

interface Props {
  data: TopSupplier[];
}

export const TopSuppliersChart = ({ data }: Props) => (
  <TopChart
    title="Топ-5 постачальників"
    data={data.map(item => ({
      name: item.name,
      totalAmount: item.totalAmount,
      count: item.awardCount,
    }))}
    countLabel="Awards"
    color="#10b981"
  />
);