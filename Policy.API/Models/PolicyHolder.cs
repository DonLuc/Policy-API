using System.ComponentModel.DataAnnotations;

namespace Policy.API.Models
{
    public class PolicyHolder
    {
        [Key]
        public Guid id { get; set; }
        public string IdNumber { get; set; }
        public string Inititals { get; set; }
        public string Surname { get; set; }
        public String dob { get; set; }
        public Gender gender { get; set; }
    }
}
