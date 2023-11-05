using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.Helpers;
using NKRY_API.ResourceParameters;
using static Azure.Core.HttpHeader;

namespace NKRY_API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/employees/{employeeId}/Finance")]
    public class EmployeesFinancesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employees;

        public EmployeesFinancesController(IUnitOfWork unitOfWork)
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

            var employeeFinance = _employees.GetEmployeeFinanceByYearAndMonth(curEmployee.Id, year, month);


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

            var curEmployee = _employees.GetEmployeeWithAllFinances(employeeId);

            if (curEmployee == null) 
            {
                return NotFound("This employee doesn't exist");
            }

            EmployeeFinance curMonthFinance = curEmployee.MonthlyFinance.Where(f => f.DueYear == employeeFinance.DueYear && f.DueMonth == employeeFinance.DueMonth).FirstOrDefault();


            if (curMonthFinance == null) 
            {
                EmployeeFinance ef = new() 
                {
                    DeliveriesMade = employeeFinance.DeliveriesMade,
                    TotalSalary = employeeFinance.TotalSalary,
                    DueMonth = employeeFinance.DueMonth,
                    DueYear = employeeFinance.DueYear,
                    EmployeeId = employeeId
                };

                
                _unitOfWork.EmployeeFinance.Create(ef);
                await _unitOfWork.Complete();
                
            } 


            int newDeliveriesMade = (int)employeeFinance.DeliveriesMade;
            try
            {
                employeeFinance.TotalSalary = FinanceHelper.CalculateTotalSalary(newDeliveriesMade, (decimal)curEmployee.FixedFinance.DeliveryRate, (decimal)curEmployee.FixedFinance.BaseSalary);
            }
            catch (NullReferenceException)
            {
                // return badrequest, create a fixedFinance for this employee first, 
                // TODO: add an endpoint for updating the fixed salary 
                await Console.Out.WriteLineAsync("Exception occured");
                employeeFinance.TotalSalary = 0;
            }

            _unitOfWork.EmployeeFinance.Update(employeeFinance);
            await _unitOfWork.Complete();

            return NoContent();
        }


        [HttpGet("Expenses")]
        public ActionResult<IEnumerable<Expense>> GetEmployeeExpenses([FromRoute] Guid employeeId, [FromQuery] EmployeeFinanceResourceParams employeeFinanceResourceParams, [FromQuery] bool names)
        {
            IEnumerable<Expense> res = _unitOfWork.Expense.GetAll();
            if (names && res?.Count() != 0)
            {
                List<string> namesList = res.Select(e => e.Name).Distinct().ToList();
                return Ok(namesList);
            }
            // default behavior is to return the current month and year's finance, other than that go and sepcify 
            int year = (int)employeeFinanceResourceParams.Year;
            int month = (int)employeeFinanceResourceParams.Month;
            if (year > 0 && month > 0)
            {
                IEnumerable<Expense> result = _employees.GetEmployeeExpensesByYearAndMonth(employeeId, year, month);
                return Ok(result);
            }

            return Ok(res);

        }

        [HttpPost("Expenses")]
        public async Task<ActionResult<Expense>> CreateEmployeeExpense([FromRoute] Guid employeeId, [FromBody] Expense expense)
        {
            if (expense == null)
            {
                return BadRequest();
            }

            var employee = _employees.GetById(employeeId);
            if (employee == null)
            {
                return NotFound("Employee with specified Id doesn't exist!");
            }

            int year = expense.DueDate.Year;
            int month = expense.DueDate.Month;

            EmployeeFinance employeeFinance = _employees.GetEmployeeFinanceByYearAndMonth(employeeId, year, month);
            IEnumerable<Expense> employeeExpenses = employeeFinance.MonthlyExpnenses;

            if (employeeExpenses == null)
            {
                employeeExpenses = new List<Expense>();
            }
            var x = (employeeExpenses.GetType());
            // TODO: This logic starting from under here, needs fucking revision, a lot of things are not right about it
            // Check if an expense with the same name already exists for the employee
            var existingExpense = employeeExpenses.FirstOrDefault(e => e.Name == expense.Name);

            if (existingExpense != null)
            {
                // Update the existing expense with the new values
                existingExpense.Amount = expense.Amount;
                existingExpense.DueDate = expense.DueDate;
                //existingExpense.PaymentType = expense.PaymentType;
            }
            else
            {
                // If no expense with the same name exists, add the new expense to the list
                employeeExpenses.ToList().Add(expense); 
            }

            employeeFinance.MonthlyExpnenses.Add(expense);

            _unitOfWork.EmployeeFinance.Update(employeeFinance);

            await _unitOfWork.Complete();

            return CreatedAtAction("GetEmployeeExpenses", new { id = expense.Id }, expense);


        }
    }
}