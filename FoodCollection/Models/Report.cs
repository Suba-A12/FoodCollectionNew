using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCollection.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }
        public string ReportTitle { get; set; }
        public DateOnly Date {  get; set; }
        public int StaffId { get; set; }
        [ForeignKey("StaffId")]
        public Staff Staff { get; set; }
    }
}
