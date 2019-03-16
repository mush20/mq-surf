using System;

namespace Mq.Shared.Messages
{
    public class BaseMessage
    {
        public Guid Id { get; private set; }
        public DateTime Created { get; private set; }

        public BaseMessage()
        {
            Id = new Guid();
            Created = DateTime.Now;
        }
    }
}