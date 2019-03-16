using Mq.Shared.Models;

namespace Mq.Shared.Messages
{
    public class CreateProductMessage: BaseMessage
    {
        public Product Product { get; set; }

        public CreateProductMessage(Product product)
        {
            Product = product;
        }
    }
}