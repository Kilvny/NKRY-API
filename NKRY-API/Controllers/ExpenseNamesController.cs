using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseNamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseNamesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }


        [HttpGet("Fixed")]
        public ActionResult<IEnumerable<string>> GetAllFixed()
        {
            IEnumerable<ExpenseNames> expenseNames = _unitOfWork.ExpenseNames.GetAll();
            var names = expenseNames.Where(e => e.Monthly == false).Select(x => x.Name).ToList();
            return Ok(names);
        }

        [HttpGet("Monthly")]
        public ActionResult<IEnumerable<string>> GetAllMonthly()
        {
            IEnumerable<ExpenseNames> expenseNames = _unitOfWork.ExpenseNames.GetAll();
            var names = expenseNames.Where(e => e.Monthly == true).Select(x => x.Name).ToList();
            return Ok(names);
        }


        [HttpPost]
        public IActionResult Post(ExpenseNames expense)
        {
            if (expense == null)
            {
                return BadRequest();
            }

            string name = expense.Name?.ToLower()?.Trim();
            
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name must be provided");
            }

            IEnumerable<string> names = _unitOfWork.ExpenseNames.GetAll().Select(e => e.Name.ToLower()).ToList();
            if (names.Contains(name))
            {
                return BadRequest("Expense name is already there");
            }

            try
            {

                ExpenseNames expenseNames = new ExpenseNames()
                {
                    Name = name,
                    Monthly = expense.Monthly
                };
                _unitOfWork.ExpenseNames.Create(expenseNames);
                _unitOfWork.Complete();
                return StatusCode(201);

            }
            catch (Exception ee)
            {
                return StatusCode(500, $"Internal Server Error with this message {ee.Message}");
            }
        }
    }
}
