namespace Tamam.Application.Common;

public class Result<T> : Result
{
    private Result(ResultStatus status, T? data, string? message, IReadOnlyList<string>? errors) : base(status, message, errors)
    {
        Data = data;
    }

    public T? Data { get; }

    public static Result<T> Ok(T data, string? message = null) => new(ResultStatus.Ok, data, message, null);

    public static Result<T> Created(T data, string? message = null) => new(ResultStatus.Created, data, message, null);

    public static new Result<T> BadRequest(params string[] errors) => new(ResultStatus.BadRequest, default, "Bad request.", errors);

    public static new Result<T> BadRequest(IEnumerable<string> errors) => new(ResultStatus.BadRequest, default, "Bad request.", errors.ToList());

    public static new Result<T> Unauthorized(string? message = null) => new(ResultStatus.Unauthorized, default, message ?? "Unauthorized.", null);

    public static new Result<T> Forbidden(string? message = null) => new(ResultStatus.Forbidden, default, message ?? "Forbidden.", null);

    public static new Result<T> NotFound(string? message = null) => new(ResultStatus.NotFound, default, message ?? "Resource was not found.", null);

    public static new Result<T> Conflict(string? message = null) => new(ResultStatus.Conflict, default, message ?? "Conflict.", null);

    public static new Result<T> Gone(string? message = null) => new(ResultStatus.Gone, default, message ?? "The resource is no longer available.", null);

    public static new Result<T> UnprocessableEntity(params string[] errors) => new(ResultStatus.UnprocessableEntity, default, "Validation failed.", errors);

    public static new Result<T> InternalServerError(string? message = null) => new(ResultStatus.InternalServerError, default, message ?? "An unexpected error occurred.", null);
}
