using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using NUnit.Framework;
using Worker.Implementation;
using Worker.Interface;
using Worker.logger;

namespace Worker.Logger.Test
{

    public class Handler : IHander<StringMessage>
    {
        public void Handle(StringMessage message, CancellationToken cancellationToken)
        {

        }
    }

    public class StringMessage : Message
    {
    }

    public class LoggerInterceptorTest
    {
        [Test]
        public void Run()
        {

            var worker = WorkerOption.New
                .EnableInterceptor()
                .CreateWorker<IWorker>();

            worker.AddHandler(new Handler());
            worker.Publish(new StringMessage());
            worker.Start();
            worker.WaitUntilNoMessage();
            worker.Stop();
        }
    }
}

