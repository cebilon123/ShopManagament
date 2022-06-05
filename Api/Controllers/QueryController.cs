using System.IO;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly QueryService _queryService;

        public QueryController(QueryService queryService)
        {
            _queryService = queryService;
        }

        public class Query
        {
            public string Sql { get; set; }
        }
        
        [HttpPost]
        public IActionResult ExecuteQuery([FromBody] Query query)
        {
            var excelStream = _queryService.ExecuteQuery(query.Sql);
            return File(excelStream.ToArray(), "application/octet-stream", $"result.csv");
        }
    }
}