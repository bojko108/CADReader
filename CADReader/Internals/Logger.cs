using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD.Internals
{
    internal sealed class Logger
    {
        public List<string> Messages { get; private set; } = new List<string>();
        public static Logger Instance => _instance;

        private static readonly Logger _instance = new Logger();
        private string Now => $"{DateTime.Now:dd.MM.yyyy HH:mm:ss}";

        public void Reset()
            => this.Messages = new List<string>();

        public void LogError(string message)
            => this.AddMessage(LogMessageType.Error, message);

        public void LogError(Exception exception)
        {
            string message = this.ExtractMessage(exception, exception.Message);
            this.AddMessage(LogMessageType.Error, message);
        }

        public void LogInfo(string message)
            => this.AddMessage(LogMessageType.Info, message);

        private void AddMessage(LogMessageType type, string message)
            => this.Messages.Add($"[{type.ToString().ToUpper()}] {Now}: {message}");

        private string ExtractMessage(Exception exception, string message)
        {
            if (exception == null)
                return message;

            message = $"{message}{Environment.NewLine}   {exception.GetType()}: {exception.Message}{Environment.NewLine}   {exception.StackTrace}";
            return this.ExtractMessage(exception.InnerException, message);
        }
    }
}
