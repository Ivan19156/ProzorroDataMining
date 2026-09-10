export interface BudgetSavings {
  totalBudget: number;
  totalContracts: number;
  totalSavings: number;
}

export interface TopBuyer {
  name: string;
  totalAmount: number;
  contractCount: number;
}

export interface TopSupplier {
  name: string;
  totalAmount: number;
  awardCount: number;
}