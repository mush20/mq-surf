using Mq.Shared.Models;

namespace Mq.Shared.Messages
{
    public class CreateProductResponseMessage: CreateProductMessage
    {
        public CreateProductResultTypes Result { get; set; }

        public CreateProductResponseMessage(Product product, CreateProductResultTypes result) : base(product)
        {
            Result = result;
        }
    }

    public enum CreateProductResultTypes
    {
        Ok, 
        Nok
    }
}