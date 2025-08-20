using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCollection.Models
{
    public class BookPickupDetail
    {
        [Key]
        public int BookPickupDetailId { get; set; }
        public int QuantityLeft { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int BookPickupId { get; set; }
        [ForeignKey("BookPickupId")]
        public BookPickup BookPickup { get; set; }
        public int FoodItemId { get; set; }
        [ForeignKey("FoodItemId")]
        public FoodItem FoodItem { get; set; }
    }
}
