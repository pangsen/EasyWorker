# EasyWorker

<pre>
<code>
using (var worker = new Worker())
{
    worker.AddHandler(new IntMessageHandler());
    worker.AddHandler(new StringMessageHandler());
    worker.AddHandler(new SecondStringMessageHandler());
    worker.Start();


    worker.Publish(Enumerable.Range(1, 1000).Select(a => new StringMessage { Message = $"String Message:{a}" }));
    worker.Publish(Enumerable.Range(1, 1000).Select(a => new IntMessage { Message = a }));
    worker.WorkUntilComplete();
}

</code>
</pre>
