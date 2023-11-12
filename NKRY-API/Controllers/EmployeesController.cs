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
        private readonly IGenericRepository<PersonalDetails> _personalDetails;
        private readonly ApplicationContext _db;

        public EmployeesController(IUnitOfWork unitOfWork, ApplicationContext applicationContext)
        {
            _unitOfWork = unitOfWork;
            _employee = unitOfWork.Employee;
            _car = unitOfWork.Car;
            _personalDetails = _unitOfWork.PersonalDetails;
            _db = applicationContext;
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
        public async Task<ActionResult<Employee>> GetEmployee(string id)
        {
          if (_employee == null)
          {
              return NotFound();
          }
            Guid _id;
            bool x = Guid.TryParse(id, out _id);
            if (!x)
            {
                return BadRequest("Id is bad");
            }
            var employee = _employee.GetById(_id);

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

            using(var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    FixedFinance ff = new();

                    if (employee.Car != null)
                    {
                        var employeeWithCar = new Employee()
                        {
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Address = employee.Address,
                            PhoneNumber = employee.PhoneNumber,
                            Photo = employee.Photo,
                            Job = employee.Job,
                            Nationality = employee.Nationality,
                            EmployeeIdNumber = employee.EmployeeIdNumber,
                            PassportNumber = employee.PassportNumber,
                        };

                        FixedFinance ffi = new()
                        {
                            EmployeeId = employeeWithCar.Id,
                            BaseSalary = employee.FixedFinance.BaseSalary,
                            DeliveryRate = employee.FixedFinance.DeliveryRate
                        };
                        employee.FixedFinance = ffi;

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
                        ff = ffi;

                    }

                    _employee.Create(employee);
                    await _unitOfWork.Complete();
                    ff.EmployeeId = employee.Id;
                    _unitOfWork.Finance.Create(ff);
                    await _unitOfWork.Complete();

                    await transaction.CommitAsync(); // If everything is successful, commit the transaction

                    return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);

                }
                catch (Exception e)
                {

                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error while creating an employee with the following exception: {e.InnerException?.Message}");
                }
            }
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

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/PersonalDetails")]
        public async Task<IActionResult> UpdatePersonalDetails(Guid id, PersonalDetails details)
        {
            if (_employee == null)
            {
                return Problem("Entity set 'ApplicationContext.employees'  is null.");
            }

            Employee employee = _employee.GetById(id);
            if(employee == null)
            {
                return BadRequest("No Such Employee");
            }
            _db.Entry(employee).State = EntityState.Detached;
            PersonalDetails employeePersonalDetails = 
                _personalDetails
                .QueryableNoTracking
                .Where(p => p.EmployeeId == id)
                .FirstOrDefault();

            try
            {
                if (employeePersonalDetails != null)
                {
                    UpdatePersonalDetails(employeePersonalDetails, details);
                }
                else
                {
                    CreateNewPersonalDetails(employee.Id, details);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"an error while updating the personal detials of employee, {ex.Message}");
            }

            await _unitOfWork.Complete();
            return NoContent();

        }

        private bool EmployeeExists(Guid id)
        {
            return (_employee?.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void UpdatePersonalDetails(PersonalDetails existingDetails, PersonalDetails newDetails)
        {

            // Update only non-null values
            existingDetails.VisaExpiryDate = newDetails.VisaExpiryDate ?? existingDetails.VisaExpiryDate;
            existingDetails.FlightTicketsDueDate = newDetails.FlightTicketsDueDate ?? existingDetails.FlightTicketsDueDate;
            existingDetails.DateOfBirth = newDetails.DateOfBirth ?? existingDetails.DateOfBirth;
            existingDetails.DuesPayDate = newDetails.DuesPayDate ?? existingDetails.DuesPayDate;
            _db.personalDetails.Attach(existingDetails);

            _personalDetails.Update(existingDetails);
        }

        private void CreateNewPersonalDetails(Guid employeeId, PersonalDetails details)
        {
            PersonalDetails personalDetails = new PersonalDetails
            {
                EmployeeId = employeeId,
                VisaExpiryDate = details.VisaExpiryDate,
                FlightTicketsDueDate = details.FlightTicketsDueDate,
                DateOfBirth = details.DateOfBirth,
                DuesPayDate = details.DuesPayDate,
            };

            _personalDetails.Create(personalDetails);
            
        }
    }
}
