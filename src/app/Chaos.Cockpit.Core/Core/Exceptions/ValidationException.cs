﻿using System;
using System.Runtime.Serialization;

namespace Chaos.Cockpit.Core.Core.Exceptions
{
  public class ValidationException : Exception
  {
    public ValidationException()
    {
    }

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}