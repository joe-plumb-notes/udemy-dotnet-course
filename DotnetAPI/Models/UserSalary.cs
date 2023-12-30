using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models
{
    public partial class UserSalary
    {
        public int UserId { get; set; }
        public decimal Salary { get; set; }
        [NotMapped]
        public decimal AvgSalary { get; set; }
    }
}