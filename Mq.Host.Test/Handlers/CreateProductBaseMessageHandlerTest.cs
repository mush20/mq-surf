using Microsoft.Extensions.Logging;
using Moq;
using Mq.Host.Data;
using Mq.Host.Handlers;
using Mq.Shared.Handlers;
using Mq.Shared.Messages;
using Mq.Shared.Models;
using Mq.Shared.Services;
using NUnit.Framework;

namespace Mq.Host.Test.Handlers
{
    public class CreateProductBaseMessageHandlerTest
    {
        private IBaseMessageHandler<CreateProductMessage, CreateProductResponseMessage> _handler;
        private Mock<IProductContext> _productContextResultOk;
        private Mock<IProductContext> _productContextResultNok;
        private ILogger<CreateProductBaseMessageHandler> _handlerLogger;
        
        
        [SetUp]
        public void Setup()
        {
            var productContextLogger = new Mock<ILogger<ProductContext>>().Object;
            _productContextResultOk = new Mock<IProductContext>();
            _productContextResultOk.Setup(x => x.SaveChanges(It.IsAny<Product>())).Returns(() => new Product());
            
            _productContextResultNok = new Mock<IProductContext>();
            _productContextResultNok.Setup(x => x.SaveChanges(It.IsAny<Product>())).Returns(() => null);
            
            _handlerLogger = new Mock<ILogger<CreateProductBaseMessageHandler>>().Object;
            
        }
        
        [Test]
        public void TestHandlerGivenOkResponse()
        {
            _handler = new CreateProductBaseMessageHandler(_productContextResultOk.Object, _handlerLogger);
            var message = new CreateProductMessage(new Product());
            var response = _handler.Handle(message);

            Assert.AreEqual(response.Result, CreateProductResultTypes.Ok);
        }

        [Test]
        public void TestHandlerGivenNOkResponse()
        {
            _handler = new CreateProductBaseMessageHandler(_productContextResultNok.Object, _handlerLogger);
            var message = new CreateProductMessage(new Product());
            var response = _handler.Handle(message);

            Assert.AreEqual(response.Result, CreateProductResultTypes.Nok);
        }
    }
}