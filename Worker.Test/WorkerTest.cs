using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void Handle(StringMessage message)
        {
            Console.WriteLine(JsonConvert.SerializeObject(message));
        }
    }
    public class SecondStringMessageHandler : IHander<StringMessage>
    {
        public void Handle(StringMessage message)
        {
            Console.WriteLine("SecondStringMessageHandler:" + JsonConvert.SerializeObject(message));
        }
    }
    public class IntMessageHandler : IHander<IntMessage>
    {

        public void Handle(IntMessage message)
        {
            Console.WriteLine(JsonConvert.SerializeObject(message));
        }

    }
    public class WorkerTest
    {
        [Test]
        public void Test()
        {
            var worker = new Worker();

            worker.AddHandler(new IntMessageHandler());
            worker.AddHandler(new StringMessageHandler());
            worker.AddHandler(new SecondStringMessageHandler());

            worker.AddJob(new StringMessage() { Message = "strting Message" });
            worker.AddJob(new IntMessage() { Message = 100 });

            Task.Run(() => worker.Start());
            while (worker.HasValue())
            {
                Task.Delay(TimeSpan.FromMilliseconds(10)).GetAwaiter().GetResult();
            }
            worker.Stop();
        }
    }
}
