using System;

namespace Paradigm.ORM.DbPublisher.Logging
{
    public class ConsoleLoggingService: ILoggingService
    {
        public void Write(string text)
        {
            Write(text, ConsoleColor.Gray, ConsoleColor.Black);
        }

        public void WriteLine(string text)
        {
            Write(text + Environment.NewLine, ConsoleColor.Gray, ConsoleColor.Black);
        }

        public void Error(string text)
        {
            Write("ERROR: " + text + Environment.NewLine, ConsoleColor.Red, ConsoleColor.Black, true);
        }

        public void Warning(string text)
        {
            Write("WARNING: " + text + Environment.NewLine, ConsoleColor.Yellow, ConsoleColor.Black);
        }

        public void Notice(string text)
        {
            Write(text + Environment.NewLine, ConsoleColor.Green, ConsoleColor.Black);
        }

        private static void Write(string text, ConsoleColor foreground, ConsoleColor background, bool error = false)
        {
            var oldForeground = Console.ForegroundColor;
            var oldBackground = Console.BackgroundColor;

            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;

            if (!error)
                Console.Write(text);
            else
                Console.Error.Write(text);

            Console.ForegroundColor = oldForeground;
            Console.BackgroundColor = oldBackground;
        }
    }
}