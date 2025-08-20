using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCollection.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public DateOnly Date {  get; set; }
        public int BookPickupId { get; set; }
        [ForeignKey("BookPickupId")]
        public BookPickup BookPickup { get; set; }
    }
}
