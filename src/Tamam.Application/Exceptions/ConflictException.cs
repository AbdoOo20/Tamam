using Tamam.Application.Common;

namespace Tamam.Application.Exceptions;

public class ConflictException(string message = "Conflict.") : AppException(ResultStatus.Conflict, message);
