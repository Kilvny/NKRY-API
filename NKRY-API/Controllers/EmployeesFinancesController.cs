using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NKRY_API.DataAccess.EFCore;
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
        private readonly ApplicationContext _db;

        public EmployeesFinancesController(IUnitOfWork unitOfWork, ApplicationContext applicationContext)
        {
            _unitOfWork = unitOfWork;
            _employees = unitOfWork.Employee;
            _db = applicationContext;
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

 
            _unitOfWork.Complete();
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
            // begining of the transaction
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var curEmployee = _employees.QueryableNoTracking?.Where(e => e.Id == employeeId)?
                       .Include(e => e.MonthlyFinance)?
                       .ThenInclude(e => e.MonthlyExpenses)
                       .Include(e => e.FixedFinance)
                       .Include(e => e.FixedExpnenses)
                       .FirstOrDefault();

                    if (curEmployee == null)
                    {
                        return NotFound("This employee doesn't exist");
                    }

                    EmployeeFinance curMonthFinance = curEmployee.MonthlyFinance.Where(f => f.DueYear == employeeFinance.DueYear && f.DueMonth == employeeFinance.DueMonth).FirstOrDefault();

                    bool isFirstEntryInTheMonth = false;
                    if (curMonthFinance == null)
                    {
                        EmployeeFinance ef = new()
                        {
                            DueMonth = employeeFinance.DueMonth,
                            DueYear = employeeFinance.DueYear,
                            TotalSalary = curEmployee?.FixedFinance?.BaseSalary,
                            EmployeeId = employeeId
                        };
                        isFirstEntryInTheMonth = true;
                        _unitOfWork.EmployeeFinance.Create(ef);
                        curMonthFinance = ef;
                    }

                    int newDeliveriesMade = (int)employeeFinance.DeliveriesMade - (int)curMonthFinance.DeliveriesMade; // we want to catch the new deliveries only
                    if (newDeliveriesMade < 0)
                    {
                        return BadRequest("Please make sure to send the correct number of total deliveries made");
                    }
                    curMonthFinance.DeliveriesMade = (int)employeeFinance.DeliveriesMade;
                    try
                    {
                        if (curMonthFinance.TotalSalary == 0)
                        {
                            curMonthFinance.TotalSalary = (decimal)curEmployee.FixedFinance.BaseSalary;
                        }
                        var currSalary = (decimal)curMonthFinance.TotalSalary;
                        var employeeDeliveryRate = (decimal)curEmployee.FixedFinance.DeliveryRate;
                        var totalSalary = FinanceHelper.CalculateTotalSalary(newDeliveriesMade, employeeDeliveryRate, currSalary);
                        curMonthFinance.TotalSalary = totalSalary;
                    }
                    catch (NullReferenceException)
                    {
                        await Console.Out.WriteLineAsync("Exception occured");
                        curMonthFinance.TotalSalary = 0;
                    }


                    if(isFirstEntryInTheMonth)
                    {
                        _unitOfWork.EmployeeFinance.Create(curMonthFinance);
                    }
                    else
                    {
                    _unitOfWork.EmployeeFinance.Update(curMonthFinance);
                    }

                    await _unitOfWork.Complete();
                    scope.Complete();

                }
                catch (Exception ex)
                {

                    return StatusCode(500, $"An error occurred while processing the request. {ex.Message} \n");
                }
            }
               

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

        [HttpPost("Expenses")] // for monthly expenses
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

            int year = (int)expense.DueDate?.Year;
            int month = (int)expense.DueDate?.Month;


            IEnumerable<Expense> employeeExpenses = new List<Expense>();
            EmployeeFinance employeeFinance = _employees.GetEmployeeFinanceByYearAndMonth(employeeId, year, month);
            if (employeeFinance == null)
            {
                EmployeeFinance ef = new()
                {
                    DueMonth = month,
                    DueYear = year,
                    MonthlyExpenses = new List<Expense>(),
                    EmployeeId = employeeId,
                };
                _unitOfWork.EmployeeFinance.Create(ef);
                await _unitOfWork.Complete();
                employeeFinance = ef;
                employeeExpenses = ef.MonthlyExpenses.ToList();
            } 
            else
            {
                employeeExpenses = employeeFinance.MonthlyExpenses;  
            }


            if (employeeExpenses == null)
            {
                employeeExpenses = new List<Expense>();
            }
            // TODO: This logic starting from under here, needs fucking revision, a lot of things are not right about it
            // Check if an expense with the same name already exists for the employee
            var existingExpense = employeeExpenses.Where(ex => ex.IsFixed == false).FirstOrDefault(e => e.Name == expense.Name);

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

            employeeFinance.MonthlyExpenses.Add(expense);

            _unitOfWork.EmployeeFinance.Update(employeeFinance);
            _unitOfWork.Expense.Create(expense);
            await _unitOfWork.Complete();

            return StatusCode(201, expense);


        }

        [HttpPost("Expenses/Fixed")]
        public async Task<ActionResult<Expense>> CreateEmployeeFixedExpense([FromRoute] Guid employeeId, [FromBody] Expense expense)
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

            Expense e = new()
            {
                Name = expense.Name,
                Amount = expense.Amount,
                DueDate = expense.DueDate,
                IsFixed = true,
                EmployeeId = employeeId
            };

            _unitOfWork.Expense.Create(e);
            await _unitOfWork.Complete();

            return StatusCode(201,e);


        }

        [HttpPost("FixedFinance")]
        public async Task<ActionResult<Expense>> EditEmployeeFixedFinance([FromRoute] Guid employeeId, FixedFinance ff)
        {
            if (ff == null) return BadRequest();

            var employee = _employees.GetById(employeeId);
            if (employee == null)
            {
                return BadRequest("Employee with specified Id doesn't exist!");
            }

            FixedFinance employeeFixedFinance = _unitOfWork.Finance.QueryableNoTracking.Where(f => f.EmployeeId == employeeId).FirstOrDefault();

            if (employeeFixedFinance == null)
            {
                FixedFinance fixedFinance = new()
                {
                    EmployeeId = employeeId,
                    BaseSalary = ff.BaseSalary,
                    DeliveryRate = ff.DeliveryRate
                };
                _unitOfWork.Finance.Create(fixedFinance);
            }
            else
            {
                employeeFixedFinance.BaseSalary = ff.BaseSalary;
                employeeFixedFinance.DeliveryRate = ff.DeliveryRate;
                _unitOfWork.Finance.Update(ff);
            }

            await _unitOfWork.Complete();
            return Ok(ff);


        }
    }
}