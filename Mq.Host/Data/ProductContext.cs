using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mq.Shared.Models;
using ServiceStack;

namespace Mq.Host.Data
{
    public class ProductContext: IProductContext
    {
        private readonly ILogger<ProductContext> _logger;
        
        public ProductContext(ILogger<ProductContext> logger)
        {
            Products = new List<Product>();
            _logger = logger;
        }
        
        public Product SaveChanges(Product product)
        {
            if (product.Name.IsEmpty() || product.Description.IsEmpty())
            {
                _logger.LogError("Error while creating a new product, invalid details");
                return null;
            }
            
            product.Id = Products.Count() + 1;
            _logger.LogInformation("New Product Created {ProductId}", product.Id);
            Products.Add(product);
            return product;
        }

        public List<Product> Products { get; private set; }
    }
}