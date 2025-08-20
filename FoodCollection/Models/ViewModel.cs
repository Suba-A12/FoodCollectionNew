namespace FoodCollection.Models
{
    public class FoodItemVM
    {
        public int FoodItemId { get; set; }
        public int CountFoodItem {  get; set; }
    }

    public class DashboardVM
    {
        public List<Payment> payments { get; set; }
        public List<FoodItemVM> foodItems { get; set; }
    }
}
