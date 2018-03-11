# EasyWorker

<pre>
<code>
var list = new List<int>() { 10,  30,  50,  70,  90};
list.ForEach(maxThreadCount =>
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();
    using (var worker = new Worker(maxThreadCount))
    {
        worker.AddHandler(new IntMessageHandler());
        worker.AddHandler(new StringMessageHandler());
        worker.AddHandler(new SecondStringMessageHandler());
        worker.Start();


        worker.Publish(Enumerable.Range(1, 1000).Select(a => new StringMessage { Message = $"String Message:{a}" }));
        worker.Publish(Enumerable.Range(1, 1000).Select(a => new IntMessage { Message = a }));
        worker.WorkUntilComplete();
    }
    stopWatch.Stop();
    Console.WriteLine($"maxThreadCount:{maxThreadCount},Milliseconds:{stopWatch.ElapsedMilliseconds}");
});

//maxThreadCount:10,Milliseconds:7216
//maxThreadCount:30,Milliseconds:1018
//maxThreadCount:50,Milliseconds:461
//maxThreadCount:70,Milliseconds:197
//maxThreadCount:90,Milliseconds:89

</code>
</pre>
