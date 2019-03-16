using System.IO;
using Microsoft.Extensions.Logging;
using Mq.Shared.Messages;
using Mq.Shared.Models;
using Mq.Shared.Services;

namespace Mq.Client.MessageClients
{
    public class ProductMessageClient: IProductMessageClient
    {
        private readonly ILogger<ProductMessageClient> _logger;
        private readonly IMessageQueueService _messageQueueService;

        public ProductMessageClient(IMessageQueueService messageQueueService, ILogger<ProductMessageClient> logger)
        {
            _messageQueueService = messageQueueService;
            _logger = logger;
        }

        public Product CreateProduct(Product product)
        {

            _logger.LogInformation("Sending Create Product Message.");
            var response = _messageQueueService.Publish<CreateProductMessage, CreateProductResponseMessage>(
                new CreateProductMessage(product));

            _logger.LogInformation("Response Received.");
            if (response.Result == CreateProductResultTypes.Nok)
            {
                _logger.LogInformation("Unable to create New Product.");
                throw new InvalidDataException();
            }
            
            _logger.LogInformation("Product created successfully");
            return response.Product;

        }
    }
}