using Mq.Shared.Handlers;
using Mq.Shared.Messages;

namespace Mq.Shared.Services
{
    public interface IMessageQueueService
    {
        TR Publish<T, TR>(T message);

        void Subscribe<T, TR, TH>()
            where T: BaseMessage // Publish Message
            where TR: BaseMessage // Response Message
            where TH: IBaseMessageHandler<T, TR>; // Response Handler

        void StartListening();
    }
}