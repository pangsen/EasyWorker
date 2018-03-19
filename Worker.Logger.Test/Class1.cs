using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using NUnit.Framework;
using Worker.Interface;
using Worker.logger;

namespace Worker.Logger.Test
{
    public interface ICoding
    {
        void DoSth();
    }
    public class Coding : ICoding
    {
//        public Coding(string xx)
//        {
//            
//        }
        public virtual void DoSth()
        {
            Console.WriteLine("敲代码咯！");
        }
    }
    public class Architecture : Coding
    {
        private readonly string _xx;

        public Architecture(string xx)
        {
            _xx = xx;
        }
        public override void DoSth()
        {
            Console.WriteLine("架构设计！");
            base.DoSth();
        }
    }
    public class MyInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            before();
            if (invocation.InvocationTarget != null)
            {
                invocation.Proceed();
            }
            after();
        }

        private void before()
        {
            Console.WriteLine("需求分析！");
        }
        private void after()
        {
            Console.WriteLine("测试！");
        }
    }

    public class MyInterceptor2 : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            before();
            if (invocation.InvocationTarget != null)
            {
                invocation.Proceed();
            }
            after();
        }

        private void before()
        {
            Console.WriteLine("需求分析2！");
        }
        private void after()
        {
            Console.WriteLine("测试2！");
        }
    }
    public interface IAddItem
    {
        void AddItem();
    }

    public class AnotherItem : IAddItem
    {
        public void AddItem()
        {
            Console.WriteLine("另一个项目开始了！");
        }
    }


    public class Handler : IHander<StringMessage>
    {
        public void Handle(StringMessage message,CancellationToken cancellationToken )
        {
            
        }
    }

    public class StringMessage : IMessgae
    {
    }

    public class TestLoggerInterceptor
    {
        [Test]
        public void Run()
        {

            var worker=WorkerOption.New
                .EnableInterceptor()
                .CreateWorker();

            worker.AddHandler(new Handler());
            worker.Publish(new StringMessage());
            worker.Start();
            worker.WaitUntilNoMessage();
            worker.Stop();
            
            return;

            ClassProxy();
            ClassProxyWithTarget();
            InterfaceProxyWithoutTarget();
            InterfaceProxyWithTarget();
            InterfaceProxyWithTargetInterface();
            Mixin();
            Mixin2();
            Mixin3();
            Mixin4();
            Mixin5();
            Console.ReadKey();
        }
        static void ClassProxy()
        {
            Console.WriteLine("\n*************ClassProxy*************\n");
            var generator = new ProxyGenerator();
            var ProjectDevelopment = generator.CreateClassProxy<Coding>(
                new MyInterceptor(),
                new MyInterceptor2()
                );
            ProjectDevelopment.DoSth();

            Print(ProjectDevelopment);
        }

        static void ClassProxyWithTarget()
        {
            Console.WriteLine("\n*************ClassProxyWithTarget*************\n");
            var generator = new ProxyGenerator();
            var ProjectDevelopment = generator.CreateClassProxyWithTarget<Coding>(
                new Architecture("xxxx"),
                new MyInterceptor(),
                new MyInterceptor2()
                );
            ProjectDevelopment.DoSth();

            Print(ProjectDevelopment);
        }

        static void InterfaceProxyWithoutTarget()
        {
            Console.WriteLine("\n*************InterfaceProxyWithoutTarget*************\n");
            var generator = new ProxyGenerator();
            var ProjectDevelopment = generator.CreateInterfaceProxyWithoutTarget<ICoding>(
                new MyInterceptor(),
                new MyInterceptor2()
                );
            ProjectDevelopment.DoSth();

            Print(ProjectDevelopment);
        }

        static void InterfaceProxyWithTarget()
        {
            Console.WriteLine("\n*************InterfaceProxyWithTarget*************\n");
            var generator = new ProxyGenerator();
            var ProjectDevelopment = generator.CreateInterfaceProxyWithTarget<ICoding>(
                new Architecture("ccccccccccc"),
                new MyInterceptor(),
                new MyInterceptor2()
                );
            ProjectDevelopment.DoSth();

            Print(ProjectDevelopment);
        }

        static void InterfaceProxyWithTargetInterface()
        {
            Console.WriteLine("\n*************InterfaceProxyWithTargetInterface*************\n");
            var generator = new ProxyGenerator();
            var ProjectDevelopment = generator.CreateInterfaceProxyWithTargetInterface<ICoding>(
                new Architecture("cccccc"),
                new MyInterceptor(),
                new MyInterceptor2()
                );
            ProjectDevelopment.DoSth();

            Print(ProjectDevelopment);
        }
        static void Print(object o)
        {
            Console.WriteLine();
            Console.WriteLine("GetType()：".PadRight(30) + o.GetType());
            Console.WriteLine("GetType().BaseType：".PadRight(30) + o.GetType().BaseType);

            var compositeField = o.GetType().GetField("__target");
            Console.WriteLine("__target：".PadRight(30) + compositeField?.FieldType + ", " + compositeField?.Name);

            foreach (var interfaceType in o.GetType().GetInterfaces())
            {
                Console.WriteLine("GetType().GetInterfaces()：".PadRight(30) + interfaceType);
            }

            foreach (var a in (o as IProxyTargetAccessor).GetInterceptors())
            {
                Console.WriteLine("GetInterceptors()：".PadRight(30) + a);
            }
        }

        static void Print2(object o)
        {
            Console.WriteLine();
            Console.WriteLine("GetType()：".PadRight(30) + o.GetType());
            Console.WriteLine("GetType().BaseType：".PadRight(30) + o.GetType().BaseType);

            var compositeField = o.GetType().GetField("__target");
            Console.WriteLine("__target：".PadRight(30) + compositeField?.FieldType + ", " + compositeField?.Name);

            foreach (var field in o.GetType().GetFields())
            {
                if (field.Name.StartsWith("__mixin"))
                {
                    Console.WriteLine("GetType().GetFields()：".PadRight(30) + field?.FieldType + ", " + field?.Name);
                }
            }

            foreach (var interfaceType in o.GetType().GetInterfaces())
            {
                Console.WriteLine("GetType().GetInterfaces()：".PadRight(30) + interfaceType);
            }

            foreach (var a in (o as IProxyTargetAccessor).GetInterceptors())
            {
                Console.WriteLine("GetInterceptors()：".PadRight(30) + a);
            }
        }

        static void Mixin()
        {
            Console.WriteLine("\n*************CreateClassProxy Mixin*************\n");
            var generator = new ProxyGenerator();
            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(new AnotherItem());

            var ProjectDevelopment = generator.CreateClassProxy<Coding>(
                options,
                new MyInterceptor(),
                new MyInterceptor2()
                );
            ProjectDevelopment.DoSth();
            Console.WriteLine("\n");
            (ProjectDevelopment as IAddItem).AddItem();

            Print2(ProjectDevelopment);
        }

        static void Mixin2()
        {
            Console.WriteLine("\n*************CreateClassProxyWithTarget Mixin*************\n");
            var generator = new ProxyGenerator();
            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(new AnotherItem());

            var ProjectDevelopment = generator.CreateClassProxyWithTarget<Coding>(
                new Architecture("sssssss"),
                options,
                new MyInterceptor(),
                new MyInterceptor2()
                );

            ProjectDevelopment.DoSth();
            Console.WriteLine("\n");
            (ProjectDevelopment as IAddItem).AddItem();

            Print2(ProjectDevelopment);
        }

        static void Mixin3()
        {
            Console.WriteLine("\n*************CreateInterfaceProxyWithoutTarget Mixin*************\n");
            var generator = new ProxyGenerator();
            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(new AnotherItem());

            var ProjectDevelopment = generator.CreateInterfaceProxyWithoutTarget<ICoding>(
                options,
                new MyInterceptor(),
                new MyInterceptor2()
                );

            ProjectDevelopment.DoSth();
            Console.WriteLine("\n");
            (ProjectDevelopment as IAddItem).AddItem();

            Print2(ProjectDevelopment);
        }

        static void Mixin4()
        {
            Console.WriteLine("\n*************CreateClassProxyWithTarget Mixin*************\n");
            var generator = new ProxyGenerator();
            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(new AnotherItem());

            var ProjectDevelopment = generator.CreateClassProxyWithTarget<Coding>(
                new Architecture("sssssssssss"),
                options,
                new MyInterceptor(),
                new MyInterceptor2()
                );

            ProjectDevelopment.DoSth();
            Console.WriteLine("\n");
            (ProjectDevelopment as IAddItem).AddItem();

            Print2(ProjectDevelopment);
        }

        static void Mixin5()
        {
            Console.WriteLine("\n*************CreateInterfaceProxyWithTargetInterface Mixin*************\n");
            var generator = new ProxyGenerator();
            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(new AnotherItem());

            var ProjectDevelopment = generator.CreateInterfaceProxyWithTargetInterface<ICoding>(
                new Architecture("sssssssssssssss"),
                options,
                new MyInterceptor(),
                new MyInterceptor2()
                );

            ProjectDevelopment.DoSth();
            Console.WriteLine("\n");
            (ProjectDevelopment as IAddItem).AddItem();

            Print2(ProjectDevelopment);
        }
    }
}

