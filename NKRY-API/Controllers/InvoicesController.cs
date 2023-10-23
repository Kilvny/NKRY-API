using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.Helpers;
using NKRY_API.Utilities;

namespace NKRY_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceRepository _invoice;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public InvoicesController(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _invoice = _unitOfWork.Invoice;
            _configuration = configuration;
            _mapper = mapper;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> Getinvoices()
        {
          if (_invoice == null)
          {
              return NotFound();
          }
            return Ok(_invoice.GetAll());
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
        {
          if (_invoice == null)
          {
              return NotFound();
          }
            var invoice = _invoice.GetById(id);

            if (invoice == null)
            {
                return NotFound();
            }

            var invoiceWithOrder = new Invoice
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Date = invoice.Date,
                DueDate = invoice.DueDate,
                PriceWithVAT = invoice.PriceWithVAT,
                BillTo = invoice.BillTo,
                VATRegistrationNumber = invoice.VATRegistrationNumber,
                CommercialRegistrationNo = invoice.CommercialRegistrationNo,
                QRUrl = invoice.QRUrl,
                OrderId = invoice.OrderId,
                Order = _unitOfWork.Order.GetById((Guid)invoice.OrderId),
            };

            return invoiceWithOrder;
        }

        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(Guid id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            _invoice.Update(invoice);

            try
            {
                await _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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

        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
          if (_invoice == null)
          {
              return Problem("Entity set 'ApplicationContext.invoices'  is null.");
          }
            int existingInvoiceCount = _invoice.GetInvoiceCount();
            invoice.InvoiceNumber = InvoiceNumberGenerator.Generate(existingInvoiceCount);
            if (invoice.Order != null)
            {
                var order = new Order()
                {
                    Items = invoice.Order.Items,
                    Quantity = invoice.Order.Quantity,
                    Description = invoice.Order.Description,
                    CreatedAt = DateTime.UtcNow.AddHours(3),
                    Price = invoice.Order.Price,
                    PaidAmount = invoice.Order.PaidAmount,
                    OrderStatus = "new"
                };

                _unitOfWork.Order.Create(order);
                invoice.OrderId = order.Id;
                invoice.PriceWithVAT = PriceWithVATCalculator.Calculate(order.Price, invoice.VATRate);

            }

            invoice.Date = DateTime.UtcNow.AddHours(3);
            invoice.DueDate = DateTime.UtcNow.AddHours(3);

            string taxInvoiceUrl = TaxInvoiceUrlGenerator.Generate(_configuration["ClientUrl"], invoice.InvoiceNumber);
            invoice.QRUrl = await QRCodeGenerator.GenerateAsync(taxInvoiceUrl);

            _invoice.Create(invoice);
            await _unitOfWork.Complete();


            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            if (_invoice == null)
            {
                return NotFound();
            }
            var invoice = _invoice.GetById(id);
            if (invoice == null)
            {
                return NotFound();
            }
            var orderIdOfTheInvoice = invoice.OrderId;
            var orderOfTheInvoice = _unitOfWork.Order.GetById(orderIdOfTheInvoice?? new Guid());

            if (orderOfTheInvoice != null)
            {
                _unitOfWork.Order.Delete(orderOfTheInvoice);
            }

            _invoice.Delete(invoice);
            await _unitOfWork.Complete();

            return NoContent();
        }

        private bool InvoiceExists(Guid id)
        {
            return (_invoice.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
