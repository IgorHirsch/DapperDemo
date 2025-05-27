using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DapperDemo.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public int CompanyId { get; set; }
        [ValidateNever]
        public virtual Company Company { get; set; }
    }
}
