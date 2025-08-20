using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCollection.Models
{
    public class BookPickup
    {
        [Key]
        public int BookPickupId { get; set; }
        public string EventName { get; set; }
        public string EventAddress { get; set; }
        public DateOnly BookDate { get; set; }
        public TimeOnly BookTime { get; set; }
        public string BookStatus { get; set; }
        public double TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public ICollection<BookPickupDetail> BookPickupDetail { get; set; }
        


    }
}
