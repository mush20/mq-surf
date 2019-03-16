using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Mq.Client.MessageClients;
using Mq.Shared.Models;
using Mq.Shared.Services;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Mq.Client.Test.MessageClients
{    
    [TestFixture, Category("Integration")]
    public class ProductMessageClientTest
    {
        private ProductMessageClient _productMessageClient;
        
        [SetUp]
        public void SetUp()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            var messageQueueServiceLoggerMock = new Mock<ILogger<MessageQueueService>>();
            var messageQueueService = new MessageQueueService(serviceProviderMock.Object, messageQueueServiceLoggerMock.Object);
            var productMessageClientLoggerMock = new Mock<ILogger<ProductMessageClient>>();
            _productMessageClient = new ProductMessageClient(messageQueueService, productMessageClientLoggerMock.Object);
        }
        
        [Test]
        public void SendMessageAndCreateProduct()
        {   

            var product = new Product
            {
                Name = "Some product",
                Description = "Some product description"
            };

            var saved = _productMessageClient.CreateProduct(product);

            Assert.True(saved.Id > 0);
        }

        [Test]
        public void GivenMultipleParallelMessages()
        {
            var savedProducts = new List<Product>();


            void CreateProduct()
            {
                var product = new Product
                {
                    Name = "Some product",
                    Description = "Some product description"
                };

                var saved = _productMessageClient.CreateProduct(product);
                savedProducts.Add(saved);                
            }


            for (var i = 0; i < 10; i++)
            {
                Parallel.Invoke((Action) CreateProduct);
            }

            Assert.AreEqual(savedProducts.Count, 10);
            
            // Confirms if all products where saved and have an Id
            Assert.AreEqual(savedProducts.Count(x => x.Id == 0), 0);
        }
    }
}