using System;
using System.Runtime.Serialization;

namespace Chaos.Cockpit.Core.Core.Exceptions
{
  public class SlideClosedException : Portal.Core.Exceptions.ClientException
  {
    public SlideClosedException()
    {
    }

    public SlideClosedException(string message, string userMessage) : base(message, userMessage)
    {
    }

    public SlideClosedException(string message, string userMessage, Exception innerException) : base(message, userMessage, innerException)
    {
    }

    protected SlideClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
