namespace FoodCollection.Models
{
    public class ReportVM
    {
        public List<MonthlyRevenueVM> MonthlyRevenue { get; set; }
        public List<FoodItemVM> PopularFood { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class MonthlyRevenueVM
    {
        public string Month { get; set; }
        public decimal Amount { get; set; }
    }
}
