using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.ViewModels
{
    public class Department
    {
        public int Id { get; set; }

        [Display(Name = "Department")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
