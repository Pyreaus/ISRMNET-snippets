public class Result
{
    public Error Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public StackTrace? StackTrace { get; init; }
    
    protected internal Result(bool isSuccess, Error error, StackTrace, stackTrace)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new InvalidOperationException(nameof(error));
        }
        
        (IsSuccess, Error, StackTrace) = (isSuccess, error, stackTrace);
    }
    public static Result Success() => new(true, Error.None);
    
    public static Result Failure(Error error) => new(false, error, new StackTrace());
    
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error, new StackTrace());

    public static Result<TValue> Create<TValue>(TValue? value) => value != null ? Success(value) : Failure<TValue>(Error.NullValue);
}

public class Result<TValue> : Result // generic Result<T> class
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error) => _value = value;

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
    
    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException(
        "The value of a failure result can not be accessed.");
}
