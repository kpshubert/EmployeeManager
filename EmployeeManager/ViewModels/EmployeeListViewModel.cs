namespace EmployeeManager.ViewModels
{
    public class EmployeeListViewModel
    {
        public EmployeeViewModel CurrentEmployee { get; set; } = new EmployeeViewModel();
        public List<EmployeeViewModel> EmployeeList { get; set; } = [];

        public string FormMode { get; set; } = string.Empty;
    }
}
