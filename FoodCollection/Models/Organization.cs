using System.ComponentModel.DataAnnotations;

namespace FoodCollection.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Email { get; set; }
        public string Phone {  get; set; }
        public string Address { get; set; }
        public ICollection<DeliveryInfo> DeliveryInfos { get; set; }
    }
}
