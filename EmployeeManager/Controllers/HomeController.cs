using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManager.Data;
using EmployeeManager.Models;
using EmployeeManager.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace EmployeeManager.Controllers
{
    public class HomeController(EmployeeManagerDbContext employeeManagerDbContext) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var departmentListFromTable = await employeeManagerDbContext.TEmDepartments.ToListAsync();
            var departmentList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList("", "", "");
            var employeeList = new List<EmployeeViewModel>();
            var employeeListFromTable = await employeeManagerDbContext.TEmEmployees.ToListAsync();

            var departmentDictionary = new Dictionary<int, string>();

            foreach (var department in departmentListFromTable)
            {
                departmentDictionary.Add(department.Id, department.Name);
            }

            if (departmentListFromTable != null && departmentListFromTable.Count > 0)
            {
                departmentList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList((System.Collections.IEnumerable)departmentListFromTable, "Id", "Name");
            }

            if (employeeListFromTable != null && employeeListFromTable.Count > 0)
            {
                foreach (var employee in employeeListFromTable)
                {
                    var employeeViewModel = new EmployeeViewModel();
                    Utilities.Utilities.CopySharedPropertyValues<TEmEmployee, EmployeeViewModel>(employee, employeeViewModel);
                    employeeViewModel.EditButtonText = $"<button type='button' class='btn btn-secondary employee-edit-button' value='{employee.Id}'><span class='fa fa-user-circle-o'></span></button>";
                    employeeViewModel.DeleteButtonText = $"<button type='button' class='btn btn-warning employee-delete-button' value='{employee.Id}'><span class='fa fa-minus-circle'></span></button>";
                    employeeViewModel.DepartmentName = departmentDictionary[employee.DepartmentId];
                    employeeList.Add(employeeViewModel);
                }
            }

            var currentEmployee = new EmployeeViewModel
            {
                Departments = departmentList
            };

            var employeeListViewModel = new EmployeeListViewModel
            {
                CurrentEmployee = currentEmployee,
                EmployeeList = employeeList
            };

            return View(employeeListViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmployeeListViewModel employeeListViewModel)
        {
            if (!string.IsNullOrWhiteSpace(employeeListViewModel.FormMode))
            {
                if (employeeListViewModel.FormMode == "Add")
                {
                    var tEmEmployee = new TEmEmployee();

                    Utilities.Utilities.CopySharedPropertyValues<EmployeeViewModel, TEmEmployee>(employeeListViewModel.CurrentEmployee, tEmEmployee);
                    employeeManagerDbContext.Add(tEmEmployee);
                    var saveResult = await employeeManagerDbContext.SaveChangesAsync();
                }
                else if (employeeListViewModel.FormMode == "Delete" && employeeListViewModel.CurrentEmployee.Id > 0)
                {
                    if (employeeListViewModel.CurrentEmployee.Id != null)
                    {
                        var tEmEmployee = await employeeManagerDbContext.TEmEmployees.FindAsync(employeeListViewModel.CurrentEmployee.Id);

                        if (tEmEmployee != null)
                        {
                            employeeManagerDbContext.TEmEmployees.Remove(tEmEmployee);
                            var deleteResult = await employeeManagerDbContext.SaveChangesAsync();
                        }
                    }
                }
                else if (employeeListViewModel.FormMode == "Edit")
                {
                    var employeeFromTable = await employeeManagerDbContext.TEmEmployees.FindAsync(employeeListViewModel.CurrentEmployee.Id);

                    if (employeeFromTable != null)
                    {
                        Utilities.Utilities.CopySharedPropertyValues<EmployeeViewModel, TEmEmployee>(employeeListViewModel.CurrentEmployee, employeeFromTable);
                        employeeManagerDbContext.Update(employeeFromTable);
                        var saveResult = await employeeManagerDbContext.SaveChangesAsync();
                    }
                }
            }

            var departmentListFromTable = await employeeManagerDbContext.TEmDepartments.ToListAsync();
            var departmentList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList("", "", "");

            var departmentDictionary = new Dictionary<int, string>();

            foreach (var department in departmentListFromTable)
            {
                departmentDictionary.Add(department.Id, department.Name);
            }

            var employeeList = new List<EmployeeViewModel>();
            var employeeListFromTable = await employeeManagerDbContext.TEmEmployees.ToListAsync();

            if (departmentListFromTable != null && departmentListFromTable.Count > 0)
            {
                departmentList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList((System.Collections.IEnumerable)departmentListFromTable, "Id", "Name");
            }

            if (employeeListFromTable != null && employeeListFromTable.Count > 0)
            {
                foreach (var employee in employeeListFromTable)
                {
                    var employeeViewModel = new EmployeeViewModel();
                    Utilities.Utilities.CopySharedPropertyValues<TEmEmployee, EmployeeViewModel>(employee, employeeViewModel);
                    employeeViewModel.EditButtonText = $"<button type='button' class='btn btn-secondary employee-edit-button' value='{employee.Id}'><span class='fa fa-user-circle-o'></span></button>";
                    employeeViewModel.DeleteButtonText = $"<button type='button' class='btn btn-warning employee-delete-button' value='{employee.Id}'><span class='fa fa-minus-circle'></span></button>";
                    employeeViewModel.DepartmentName = departmentDictionary[employeeViewModel.DepartmentId];
                    employeeList.Add(employeeViewModel);
                }
            }

            employeeListViewModel.CurrentEmployee.Departments = departmentList;
            employeeListViewModel.EmployeeList = employeeList;

            return View(employeeListViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployee([FromQuery] int? Id)
        {
            var returnValue = new TEmEmployee();

            if (Id != null && Id > 0)
            {
                var employeeFromTable = await employeeManagerDbContext.TEmEmployees.Where(emp => emp.Id == Id).FirstOrDefaultAsync();

                if (employeeFromTable != null)
                {
                    returnValue = employeeFromTable;
                }
            }

            return Json(returnValue);
        }
    }
}
