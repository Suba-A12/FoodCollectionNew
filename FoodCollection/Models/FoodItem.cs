using System.ComponentModel.DataAnnotations;

namespace FoodCollection.Models
{
    public class FoodItem
    {
        [Key]
        public int FoodItemId { get; set; }
        public bool IsVegetarian{ get; set; }
        public string FoodType { get; set; }
        public ICollection<BookPickupDetail> BookPickupDetail { get; set; }
    }
}
