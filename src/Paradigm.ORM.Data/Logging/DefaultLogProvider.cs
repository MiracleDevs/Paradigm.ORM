using System;
using System.Diagnostics;

namespace Paradigm.ORM.Data.Logging
{
    /// <summary>
    /// The default logger logs messages to the debug output.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Logging.ILogProvider" />
    public class DefaultLogProvider: ILogProvider
    {
        public void Info(string message)
        {
            Debug.WriteLine($"[{DateTime.Now:yy-MM-dd hh:mm:ss}][INFO] - {message}");
        }

        public void Warning(string message)
        {
            Debug.WriteLine($"[{DateTime.Now:yy-MM-dd hh:mm:ss}][WARN] - {message}");
        }

        public void Error(string message)
        {
            Debug.WriteLine($"[{DateTime.Now:yy-MM-dd hh:mm:ss}][ERROR] - {message}");
        }
    }
}