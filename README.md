# EasyWorker

EasyWorker可以在多线程条件下帮助你分发和处理消息。

>基本用法如下

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
>实例化Worker
实例化Worker需要两个参数MaxThreadCount和DelaySecondWhenNoMessageCome,默认是分别为MaxThreadCount:10和DelaySecondWhenNoMessageCome:5.
<pre>
<code>
var worker = new Worker(maxThreadCount:10,delaySecondWhenNoMessageCome:5)
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
worker.Publish( new List<SiteProfile>());    
</code>
</pre>

>等待已经Publish的消息处理完成
<pre>
<code>
worker.WorkUntilComplete();
</code>
</pre>
