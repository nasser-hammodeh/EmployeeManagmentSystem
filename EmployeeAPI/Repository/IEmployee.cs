using EmployeeAPI.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.Repository
{
    public interface IEmployee
    {
        public Task<List<Employee>> GetEmployees();
        public Task<Employee> GetEmployee(int id);
        public Task<Employee> AddEmployee(Employee employee);
        public Task<bool> UpdateEmployee(Employee employee);
        public Task<bool> DeleteEmployee(int id);
        public Task<string> Login(LoginModel model);
        public Task<List<EmployeeStatisticalInformation>> GetEmployeeStatisticalInformation();
    }
}
