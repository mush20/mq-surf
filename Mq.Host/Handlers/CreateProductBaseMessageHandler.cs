using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mq.Host.Data;
using Mq.Shared.Handlers;
using Mq.Shared.Messages;
using ServiceStack.Messaging;

namespace Mq.Host.Handlers
{
    public class CreateProductBaseMessageHandler: IBaseMessageHandler<CreateProductMessage, CreateProductResponseMessage>
    {
        private readonly ILogger<CreateProductBaseMessageHandler> _logger;
        private readonly IProductContext _productContext;

        public CreateProductBaseMessageHandler(IProductContext productContext, ILogger<CreateProductBaseMessageHandler> logger)
        {
            _productContext = productContext;
            _logger = logger;
        }
        
        public CreateProductResponseMessage Handle(CreateProductMessage message)
        { 
            _logger.LogInformation("Handling Create Product Message {MessageId}", message.Id);
            var product = _productContext.SaveChanges(message.Product);

            var result = (product != null) ? CreateProductResultTypes.Ok : CreateProductResultTypes.Nok;
            
            _logger.LogInformation("Returning Create Product Response Message");
            return new CreateProductResponseMessage(product, result);
        }
    }
}