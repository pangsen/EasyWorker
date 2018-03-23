using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Worker.Interface;

namespace Worker.logger
{

    public class LoggerInterceptor : IInterceptor
    {
        private readonly ILogger _logger;

        public LoggerInterceptor(ILogger logger)
        {
            _logger = logger;
        }
        public void Intercept(IInvocation invocation)
        {
            _logger.Write(invocation.Method.Name);

            foreach (var invocationArgument in invocation.Arguments)
            {
                _logger.Write(JsonConvert.SerializeObject(invocationArgument,new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All}));
            }

            invocation.Proceed();
        }
    }
}
