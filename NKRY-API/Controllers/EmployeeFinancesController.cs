using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.Helpers;
using NKRY_API.ResourceParameters;

namespace NKRY_API.Controllers
{
    [ApiController]
    [Route("api/employees/{employeeId}/Finance")]
    public class EmployeeFinancesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employees;

        public EmployeeFinancesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _employees = unitOfWork.Employee;
        }

        // Get api/employees/{employeeId}/Finance
        [HttpGet]
        public ActionResult<EmployeeFinance> GetFinanceOfEmployee([FromRoute] Guid employeeId, [FromQuery] EmployeeFinanceResourceParams employeeFinanceResourceParams)
        {
            if (_employees == null)
            {
                return NotFound();
            }

            Employee? curEmployee = _employees.GetById(employeeId);
            int year = (int)employeeFinanceResourceParams.Year;
            int month = (int)employeeFinanceResourceParams.Month;


            if (curEmployee == null)
            {
                return NotFound(employeeId);
            }

            var employeeFinance = _employees.GetEmployeeFinance(curEmployee, year, month);


            return Ok(employeeFinance);
        }

        // Put: this can be used to create a new employeeFinance as well! 
        [HttpPut]
        public async Task<ActionResult> UpdateEmployeeFinance([FromRoute] Guid employeeId, [FromBody] EmployeeFinance employeeFinance)
        {
            if (employeeId != employeeFinance.EmployeeId)
            {
                return BadRequest($"{employeeId} and {employeeFinance?.EmployeeId} doesn't match");
            }

            var curEmployee = _employees.GetEmployeeByFinance(employeeId);

            if (curEmployee == null) 
            {
                return NotFound("This employee doesn't exist");
            }

            EmployeeFinance curMonthFinance = curEmployee.EmployeeFinance.Where(f => f.DueYear == employeeFinance.DueYear && f.DueMonth == employeeFinance.DueMonth).FirstOrDefault();


            if (curMonthFinance == null) 
            {
                EmployeeFinance ef = new() 
                {
                    DeliveriesMade = employeeFinance.DeliveriesMade,
                    DeliveryRate = employeeFinance.DeliveryRate,
                    BaseSalary = employeeFinance.BaseSalary,
                    TotalSalary = employeeFinance.TotalSalary,
                    DueMonth = employeeFinance.DueMonth,
                    DueYear = employeeFinance.DueYear,
                    EmployeeId = employeeId
                };

                _unitOfWork.EmployeeFinance.Create(ef);
                await _unitOfWork.Complete();
                
            }

            int newDeliveriesMade = employeeFinance.DeliveriesMade;
            employeeFinance.TotalSalary = FinanceHelper.CalculateTotalSalary(newDeliveriesMade, employeeFinance.DeliveryRate, employeeFinance.BaseSalary);

            _unitOfWork.EmployeeFinance.Update(employeeFinance);
            await _unitOfWork.Complete();

            return NoContent();
        }

    }
}