namespace WebPlayground.Core.Exceptions
{
    public class LoggableException : Exception
    {
        public bool IsLogged { get; set; }

        public LoggableException(string message, bool isLogged = false) : base(message)
        {
            IsLogged = isLogged;
        }

        public LoggableException(string message, Exception innerException, bool isLogged = false) : base(message, innerException)
        {
            IsLogged = isLogged;
        }

        public LoggableException MarkAsLogged()
        {
            IsLogged = true;
            return this;
        }
    }
}
