using System;
using System.Text;

namespace Worker.Implementation
{
    public static class ExceptionExtension
    {
        public static string ToTetailMessage(this Exception exception)
        {
            var sb = new StringBuilder();
            TetailMessage(exception, sb);
            return sb.ToString();
        }
        public static void TetailMessage(Exception exception, StringBuilder sb)
        {
            sb.AppendLine(exception.Message);
            if (exception.InnerException != null)
            {
                TetailMessage(exception.InnerException, sb);
            }
        }
    }
}