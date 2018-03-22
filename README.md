# EasyWorker

EasyWorker可以在多线程条件下帮助你分发和处理消息。

>基本用法如下

<pre>
<code>

var worker = WorkerOption
            .New
            .EnableInterceptor()
            .SetMaxTaskCount(maxThreadCount)
            .SetDelaySecondsWhenNoMessageCome(5)
            .CreateWorker();
worker.AddHandler(new IntMessageHandler());
worker.AddHandler(new StringMessageHandler());
worker.AddHandler(new SecondStringMessageHandler());
worker.Start();

worker.Publish(Enumerable.Range(1, 1000).Select(a => new StringMessage { Message = $"String Message:{a}" }));
worker.Publish(Enumerable.Range(1, 1000).Select(a => new IntMessage { Message = a }));
worker.WaitUntilNoMessage();
worker.Stop();

</code>
</pre>

>自定义消息
所有消息必须实现 IMessage 接口
<pre>
<code>
public class SiteProfile:IMessgae
{
    public string SiteAddress { get; set; }
}
</code>
</pre>

>自定义消息处理
所有消息处理函数必须实现泛型接口IHandler&lt;>
<pre>
<code>
public class SiteHandler : IHander&lt;SiteProfile>
{
    public void Handle(SiteProfile message)
    {
        throw new NotImplementedException();
    }
}
</code>
</pre>
>注册Handler
<pre>
<code>
worker.AddHandler(new SiteHandler());
</code>
</pre>
>Publish Message
<pre>
<code>
//单个
worker.Publish( new SiteProfile()));
//批量
worker.Publish( new List&lt;SiteProfile>());    
</code>
</pre>

>等待已经Publish的消息处理完成
<pre>
<code>
worker.WaitUntilNoMessage();
</code>
</pre>
>EnableIntercepto

>SetMaxTaskCount

>SetDelaySecondsWhenNoMessageCome
