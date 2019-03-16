using System.Collections.Generic;
using Mq.Shared.Models;

namespace Mq.Host.Data
{
    public interface IProductContext
    {
        List<Product> Products { get; }
        Product SaveChanges(Product product);
    }
}