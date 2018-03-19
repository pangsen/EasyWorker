using System;
using Worker.Interface;

namespace Worker.Implementation
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}