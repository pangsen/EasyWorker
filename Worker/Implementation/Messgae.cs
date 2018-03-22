using System;

namespace Worker.Implementation
{
    public abstract class Messgae
    {
        protected Messgae()
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            Status = MessgaeStatus.New;
        }
        public Guid Id { get; }

        public DateTime CreateTime { get; }

        public MessgaeStatus Status { get; set; }
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