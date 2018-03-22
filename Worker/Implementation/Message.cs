using System;

namespace Worker.Implementation
{
    public abstract class Message
    {
        protected Message()
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            Status = MessgaeStatus.New;
        }
        public Guid Id { get; }

        public DateTime CreateTime { get; }

        public MessgaeStatus Status { get; set; }
        public Message Pending()
        {
            Status = MessgaeStatus.Pending;
            return this;
        }
        public Message Processing()
        {
            Status = MessgaeStatus.Processing;
            return this;
        }
        public Message Completed()
        {
            Status = MessgaeStatus.Completed;
            return this;
        }
        public Message Error()
        {
            Status = MessgaeStatus.Error;
            return this;
        }
    }

    public enum MessgaeStatus
    {
        New,
        Pending,
        Processing,
        Completed,
        Error
    }
}