using Tamam.Application.Common;

namespace Tamam.Application.Exceptions;

public class BadRequestException(string message = "Bad request.") : AppException(ResultStatus.BadRequest, message);
