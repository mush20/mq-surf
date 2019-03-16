using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Mq.Host.Data;
using Mq.Shared.Models;

namespace Mq.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductContext _productContext;

        public ProductsController(IProductContext productContext)
        {
            _productContext = productContext;
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<List<Product>> Get()
        {
            return _productContext.Products;
        }
    }
}
