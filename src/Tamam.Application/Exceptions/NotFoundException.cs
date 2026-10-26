using Tamam.Application.Common;

namespace Tamam.Application.Exceptions;

public class NotFoundException(string message = "Resource was not found.") : AppException(ResultStatus.NotFound, message);
