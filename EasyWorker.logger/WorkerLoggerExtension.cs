using Castle.DynamicProxy;

namespace Worker.logger
{
    public static class WorkerLoggerExtension
    {

        public static WorkerOption EnableInterceptor(this WorkerOption options)
        {
            options.WorkerFactory = (ops) =>
            {
                var generator = new ProxyGenerator();
                return generator.CreateInterfaceProxyWithTarget<IWorker>(
                    new Worker(ops),
                    new LoggerInterceptor(ops.Logger));
            };
            return options;
        }



    }
}