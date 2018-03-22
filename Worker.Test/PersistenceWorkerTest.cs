using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Worker.Persistence;

namespace Worker.Test
{
    public class PersistenceWorkerTest
    {

        [Test]
        public void TestPersistenceWorker()
        {
            var worker = WorkerOption
                .New
                .UsePersistenceWorker()
                .CreateWorker() as IPersistenceWorker;

            worker.AddHandler(new IntMessageHandler());
            worker.Start();
            //
            //            worker.Publish(Enumerable.Range(1, 1000).Select(a => new IntMessage { Message =a }));
            //            worker.RePublishErrorMessages();
            worker.WaitUntilNoMessage();
            worker.Publish(Enumerable.Range(1, 220).Select(a => new IntMessage { Message = a }));
            Task.Delay(TimeSpan.FromSeconds(6)).GetAwaiter().GetResult();
            worker.Save();
            worker.Stop();
        }


    }
}