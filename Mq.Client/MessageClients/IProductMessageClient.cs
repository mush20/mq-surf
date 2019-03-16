using Mq.Shared.Models;

namespace Mq.Client.MessageClients
{
    public interface IProductMessageClient
    {
        Product CreateProduct(Product product);
    }
}