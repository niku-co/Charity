namespace NikuAPI.Entities;

public readonly struct Result<TResult, TErorr>
{
    private readonly bool _success;
    public readonly TResult Value;
    public readonly TErorr Error;

    private Result(TResult value, TErorr error, bool success)
    {
        Value = value;
        Error = error;
        _success = success;
    }

    public bool IsOk => _success;

    public static Result<TResult, TErorr> Ok(TResult v)
    {
        return new(v, default(TErorr), true);
    }

    public static Result<TResult, TErorr> Err(TErorr e)
    {
        return new(default(TResult), e, false);
    }

    public static implicit operator Result<TResult, TErorr>(TResult v) => new(v, default(TErorr), true);
    public static implicit operator Result<TResult, TErorr>(TErorr e) => new(default(TResult), e, false);

    public R Match<R>(
            Func<TResult, R> success,
            Func<TErorr, R> failure) =>
        _success ? success(Value) : failure(Error);
}
