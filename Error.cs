public class Error : IEquatable<Error>
{
    public static readonly Error NullValue = new(Errors.NullValue, "The specified result value is null.");
    public static readonly Error None = new(string.Empty, string.Empty);
    
    public Error(string code, string message)
    {
        (Code, Message) = (code, message);
    }
    public string Code { get; }
    public string Message { get; }

    public static implicit operator string(Error e) => e.Code;
    public static implicit operator Result(Error e) => Result.Failure(e);

    public static bool operator !=(Error? a, Error? b) => !(a == b);
    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        else if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }    
    public virtual bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }
    public override bool Equals(object? obj) => obj is Error error && Equals(error);
    
    public override string ToString() => Code;
    
    public override int GetHashCode() => HashCode.Combine(Code, Message);
}
