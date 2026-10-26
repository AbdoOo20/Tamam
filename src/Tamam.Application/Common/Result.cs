namespace Tamam.Application.Common;

public class Result<T>
{
    public bool Succeeded { get; private init; }

    public T? Value { get; private init; }

    public IReadOnlyList<string> Errors { get; private init; } = [];

    public static Result<T> Ok(T value) => new() { Succeeded = true, Value = value };

    public static Result<T> Fail(params IEnumerable<string> errors) => new() { Errors = errors.ToList() };
}
