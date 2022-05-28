using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        
        [HttpGet("{orderId:int}")]
        public IActionResult GetInvoiceExcelForOrder(int orderId)
        {
            var excelStream = _invoiceService.ProduceExcel(orderId);

            return File(excelStream.ToArray(), "application/octet-stream", $"OrderInvoice_{orderId}.xlsx");
        }
    }
}