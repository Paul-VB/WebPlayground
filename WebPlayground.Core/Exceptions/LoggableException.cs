namespace WebPlayground.Core.Exceptions
{
    public class LoggableException : Exception
    {
        public bool IsLogged { get; set; }
        public LoggableException(string message) : base(message)
        {
        }
        public LoggableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LoggableException MarkAsLogged()
        {
            IsLogged = true;
            return this;
        }
    }
}
