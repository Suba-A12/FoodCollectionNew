namespace FoodCollection.Models
{
    public class ReportVM
    {
        public List<MonthlyRevenueVM> MonthlyRevenue { get; set; }
        public List<PopularFoodVM> PopularFood { get; set; }
        public List<OrganizationPickupVM> OrganizationStatus { get; set; }
        public decimal TotalRevenue { get; set; }
    }
    public class MonthlyRevenueVM
    {
        public string Month { get; set; }
        public decimal Amount { get; set; }
    }

    public class PopularFoodVM
    {
        public int FoodItemId { get; set; }
        public string FoodType { get; set; } 
        public int CountFoodItem { get; set; }
    }
    public class OrganizationPickupVM
    {
        public string OrganizationName { get; set; }
        public int PickupCount { get; set; }
    }
}
