using System.IO;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly UploadService _uploadService;

        public UploadController(UploadService uploadService)
        {
            _uploadService = uploadService;
        }
        
        [HttpPost("Orders")]
        public ActionResult UploadOrders(IFormFile file)
        {
            if (file.ContentType != "text/csv")
            {
                return UnprocessableEntity();
            }

            using var stream = new MemoryStream();
            file.CopyTo(stream);
            _uploadService.UploadOrders(stream);

            return Created("Orders", "");
        }

        [HttpPost("Products")]
        public ActionResult UploadProducts(IFormFile file)
        {
            if (file.ContentType != "text/csv")
            {
                return UnprocessableEntity();
            }
            
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            _uploadService.UploadProducts(stream);
            
            return Created("Products", "");
        }
        
        [HttpPost("Users")]
        public ActionResult UploadUsers(IFormFile file)
        {
            if (file.ContentType != "text/csv")
            {
                return UnprocessableEntity();
            }
            
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            _uploadService.UploadUsers(stream);
            
            return Created("Users", "");
        }
        
        [HttpPost("Addresses")]
        public ActionResult UploadAddresses(IFormFile file)
        {
            if (file.ContentType != "text/csv")
            {
                return UnprocessableEntity();
            }
            
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            _uploadService.UploadAddresses(stream);
            
            return Created("Users", "");
        }
    }
}