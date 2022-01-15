using EmployeeAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAPI.Repository
{
    public class EmployeeRepository : IEmployee
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public async Task<Employee> AddEmployee(Employee employee)
        {
            try
            {
                if (employee != null)
                {
                    employee.IsActive = Convert.ToBoolean(1);
                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();
                    return employee;
                }
                else
                {
                    return new Employee();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee != null)
                {
                    employee.IsActive = Convert.ToBoolean(0);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Employee> GetEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == Convert.ToBoolean(1));
                if (employee != null)
                {
                    return employee;
                }
                else
                {
                    return new Employee();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Employee>> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees.Where(x => x.IsActive == Convert.ToBoolean(1)).ToListAsync();
                return employees;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            try
            {
                if (employee != null)
                {
                    var existEmployee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);
                    existEmployee.FirstName = employee.FirstName;
                    existEmployee.LastName = employee.LastName;
                    existEmployee.Gender = employee.Gender;
                    existEmployee.Age = employee.Age;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> Login(LoginModel model)
        {
            var user = await _context.Users.Where(x => x.UserName == model.UserName).FirstOrDefaultAsync();
            if (user != null && user.UserName == model.UserName && model.Password == user.Password)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMonths(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456")), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return token;
            }
            else
            {
                return "";
            }
        }

        public async Task<List<EmployeeStatisticalInformation>> GetEmployeeStatisticalInformation()
        {
            var result = new List<EmployeeStatisticalInformation>();
            int maleCount = await _context.Employees.Where(x => x.Gender == "Male" && x.IsActive == Convert.ToBoolean(1)).CountAsync();

            int femaleCount = await _context.Employees.Where(x => x.Gender == "Female" && x.IsActive == Convert.ToBoolean(1)).CountAsync();

            int activeEmpCount = await _context.Employees.Where(x => x.IsActive == Convert.ToBoolean(1)).CountAsync();

            int inactiveEmpCount = await _context.Employees.Where(x => x.IsActive == Convert.ToBoolean(0)).CountAsync();

            int less20Age = await _context.Employees.Where(x => x.Age < 20 && x.IsActive == Convert.ToBoolean(1)).CountAsync();

            int btw20_30Age = await _context.Employees.Where(x => x.Age >= 20 && x.Age < 30 && x.IsActive == Convert.ToBoolean(1)).CountAsync();

            int btw30_40Age = await _context.Employees.Where(x => x.Age >= 30 && x.Age < 40 && x.IsActive == Convert.ToBoolean(1)).CountAsync();

            int btw40_50Age = await _context.Employees.Where(x => x.Age >= 40 && x.Age < 50 && x.IsActive == Convert.ToBoolean(1)).CountAsync();

            int greater50Age = await _context.Employees.Where(x => x.Age >= 50 && x.IsActive == Convert.ToBoolean(1)).CountAsync();

            result.Add(new EmployeeStatisticalInformation { title = "male employee average", value = maleCount });
            result.Add(new EmployeeStatisticalInformation { title = "female employee average", value = femaleCount });
            result.Add(new EmployeeStatisticalInformation { title = "active employees", value = activeEmpCount });
            result.Add(new EmployeeStatisticalInformation { title = "inactive employees", value = inactiveEmpCount });
            result.Add(new EmployeeStatisticalInformation { title = "age less than 20", value = less20Age });
            result.Add(new EmployeeStatisticalInformation { title = "age between 20 and 30", value = btw20_30Age });
            result.Add(new EmployeeStatisticalInformation { title = "age between 30 and 40", value = btw30_40Age });
            result.Add(new EmployeeStatisticalInformation { title = "age between 40 and 50", value = btw40_50Age });
            result.Add(new EmployeeStatisticalInformation { title = "age greater than 50", value = greater50Age });
            return result;
        }
    }
}
