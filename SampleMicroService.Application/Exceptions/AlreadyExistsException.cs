using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMicroService.Application.Exceptions;

public sealed class AlreadyExistsException(string message) : Exception(message);
