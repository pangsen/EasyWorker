using System;
using System.Diagnostics;
using Worker.Interface;

namespace Worker.Implementation
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string message)
        {
            Debug.WriteLine(message);
        }
    }
}