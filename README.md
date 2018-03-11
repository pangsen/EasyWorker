# EasyWorker

<pre>
<code>
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
</code>
</pre>
