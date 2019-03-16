using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mq.Client.MessageClients;
using Mq.Shared.Models;

namespace Mq.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IProductMessageClient _productMessageClient;

        public ValuesController(IProductMessageClient productMessageClient)
        {
            _productMessageClient = productMessageClient;
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Use Post request to test Message Broker";
        }

        // POST api/values
        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product value)
        {
            try
            {
                var saved = _productMessageClient.CreateProduct(value);
                return Created("", saved);

            }
            catch (InvalidDataException)
            {
                return UnprocessableEntity();
            }
            catch (TimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }
}
