﻿namespace Worker.Interface
{
    public interface IConsumer
    {
        int PendingTaskCount { get; set; }
        IMessageQueue MessageQueue { get; set; }
        IHandlerManager HandlerManager { get; set; }
        void Stop();
        void Start();
        void SetMaxTaskCount(int maxTaskCount);
        void SetDelaySecondsWhenNoMessageCome(int maxTaskCount);
    }

    public interface ILogger
    {
        void Write(string message);
    }
}