using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPlayground.Core.Models.Ollama;

namespace WebPlayground.Core.Exceptions
{
    public class ServiceOfflineException : LoggableException
    {
        public ServiceOfflineException(string message) : base(message)
        {
        }

        public ServiceOfflineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
