using System;
using System.Runtime.Serialization;

namespace Chaos.Cockpit.Core.Core.Exceptions
{
	public class FileParameterMissingException : Exception
	{
		public FileParameterMissingException()
		{
		}

		public FileParameterMissingException(string message) : base(message)
		{
		}

		public FileParameterMissingException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected FileParameterMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}