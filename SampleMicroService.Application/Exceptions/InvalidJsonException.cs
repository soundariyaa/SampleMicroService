using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMicroService.Application.Exceptions;

public sealed class InvalidJsonException : Exception
{
    public InvalidJsonException() { }
    public InvalidJsonException(string message) : base(message) { }
    public InvalidJsonException(string message, Exception innerException) : base(message, innerException) { }
}
