using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace EmployeeManager.ViewModels
{
    public class EmployeeViewModel
    {

        public int Id { get; set; }

        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;


        [Display(Name = "Email")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone")]
        [StringLength(12)]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Department")]
        public string? DepartmentName { get; set; } = string.Empty;

        public string? EditButtonText = string.Empty;

        public string? DeleteButtonText = string.Empty;

        public SelectList Departments { get; set; } = new SelectList("", "", "");
    }
}