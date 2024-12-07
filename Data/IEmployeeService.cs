using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazorpdds.Data
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeModel>> GetEmployees();
        Task<bool> CreateEmployee(Employee employee);
        Task<bool> EditEmployee(int id, EmployeeModel employee);
        Task<EmployeeModel> SingleEmployee(int id);
        Task<bool> DeleteEmployee(int id);
    }
}
