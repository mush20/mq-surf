using Mq.Shared.Messages;

namespace Mq.Shared.Handlers
{
    public interface IBaseMessageHandler<in T, out TR>: IBaseMessageHandler
        where T: BaseMessage
        where TR: BaseMessage
    {
        TR Handle(T message);
    }

    public interface IBaseMessageHandler
    {
    }
}