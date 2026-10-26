using Tamam.Application.Common;

namespace Tamam.Application.Exceptions;

public class ForbiddenException(string message = "Forbidden.") : AppException(ResultStatus.Forbidden, message);
