using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Worker.Interface;
using Worker.logger;

namespace Worker.Test
{
    public class StringMessage : IMessgae
    {
        public string Message { get; set; }
    }

    public class IntMessage : IMessgae
    {
        public int Message { get; set; }
    }

    public class StringMessageHandler : IHander<StringMessage>
    {
        public void Handle(StringMessage message, CancellationToken cancellationToken)
        {

            //            Console.WriteLine(JsonConvert.SerializeObject(message));
        }
    }
    public class SecondStringMessageHandler : IHander<StringMessage>
    {
        public void Handle(StringMessage message, CancellationToken cancellationToken)
        {
            //            Console.WriteLine("SecondStringMessageHandler:" + JsonConvert.SerializeObject(message));
        }
    }
    public class IntMessageHandler : IHander<IntMessage>
    {

        public void Handle(IntMessage message, CancellationToken cancellationToken)
        {
            //            Console.WriteLine(JsonConvert.SerializeObject(message));
        }

    }
    public class WorkerTest
    {
        [Test]
        public void Test()
        {
            var list = new List<int>() { 10, 30, 50, 70, 90 };
            list.ForEach(maxThreadCount =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var worker = WorkerOption
                .New
                .EnableInterceptor()
                .SetMaxTaskCount(maxThreadCount)
                .CreateWorker();
                worker.AddHandler(new IntMessageHandler());
                worker.AddHandler(new StringMessageHandler());
                worker.AddHandler(new SecondStringMessageHandler());
                worker.Start();

                worker.Publish(Enumerable.Range(1, 1000).Select(a => new StringMessage { Message = $"String Message:{a}" }));
                worker.Publish(Enumerable.Range(1, 1000).Select(a => new IntMessage { Message = a }));
                worker.WaitUntilNoMessage();
                worker.Stop();
                Console.WriteLine($"maxThreadCount:{maxThreadCount},Milliseconds:{stopWatch.ElapsedMilliseconds}");
            });

        }
    }
}
