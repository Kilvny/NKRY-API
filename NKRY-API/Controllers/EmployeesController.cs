using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employee;
        private readonly ICarRepository _car;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _employee = unitOfWork.Employee;
            _car = unitOfWork.Car;
        }

        // GET: api/Employees
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
          if (_employee== null)
          {
              return NotFound();
          }
            return Ok(_employee.GetAll());
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
          if (_employee == null)
          {
              return NotFound();
          }
            var employee = _employee.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(Guid id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _employee.Update(employee);

            try
            {
                await _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
          if (_employee == null)
          {
              return Problem("Entity set 'ApplicationContext.employees'  is null.");
          }

            if(employee.Car != null)
            {
                var employeeWithCar = new Employee()
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Address = employee.Address,
                    PhoneNumber = employee.PhoneNumber,
                    Photo = employee.Photo,

                };

                var car = new Car()
                {
                    Company = employee.Car.Company,
                    Model = employee.Car.Model,
                    ManfactureYear = employee.Car.ManfactureYear,
                    PlateNumber = employee.Car.PlateNumber
                };

                _car.Create(car);
                employeeWithCar.CarId = car.Id;
                employee = employeeWithCar;

            }

            _employee.Create(employee);
            await _unitOfWork.Complete();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            if (_employee == null)
            {
                return BadRequest();
            }
            var employeeToDelete = _employee.GetById(id);
            if (employeeToDelete == null)
            {
                return NotFound("Employee you want to delete doesn't exist!");
            }

            

            _employee.Delete(employeeToDelete);
            await _unitOfWork.Complete();

            return NoContent();
        }

        private bool EmployeeExists(Guid id)
        {
            return (_employee?.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
