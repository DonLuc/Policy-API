using System.ComponentModel.DataAnnotations;

namespace Policy.API.Models
{
    public class AvbobPolicy
    {
        [Key]
        public Guid id { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyType { get; set; }
        public DateTime CommencementDate { get; set; }
        public Double Installment { get; set; }
    }
}
