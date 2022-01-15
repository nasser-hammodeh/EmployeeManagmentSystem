using EmployeeAPI.Model;
using EmployeeAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeRepository;
        public EmployeeController(IEmployee _employeeRepository)
        {
            this._employeeRepository = _employeeRepository;
        }
        [HttpGet]
        //Get all active employees
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            //return await _context.Employees.ToListAsync();
            return await _employeeRepository.GetEmployees();

        }
        [HttpGet("{id}")]
        //Get specific employee bassed on employee id
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }
        [HttpPost]
        //Add new employee
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            var addedEmployee = await _employeeRepository.AddEmployee(employee);
            return CreatedAtAction("GetEmployee", new { id = addedEmployee.Id }, addedEmployee);
        }
        [HttpDelete("{id}")]
        //Inactive employee by change Isactive value to 0
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeRepository.DeleteEmployee(id);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut]
        //Update employee
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            var result = await _employeeRepository.UpdateEmployee(employee);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        //Authenticate login process by generation JWT if username and password is correct
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _employeeRepository.Login(model);
            if (result != "")
            {
                return Ok(new { result });
            }
            else
            {
                return BadRequest(new { message = "Usernaem or password is incorrect" });
            }
        }
        [HttpGet]
        //Get Statistical title such as male employees and the count of them  
        public async Task<IEnumerable<EmployeeStatisticalInformation>> GetEmployeeStatisticalInformation()
        {
            var result = await _employeeRepository.GetEmployeeStatisticalInformation();
            return result;
        }
    }
}
