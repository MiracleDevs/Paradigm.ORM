namespace Paradigm.ORM.DbFirst.Logging
{
    public interface ILoggingService
    {
        void Write(string text);

        void WriteLine(string text);

        void Error(string error);

        void Warning(string error);

        void Notice(string error);
    }
}