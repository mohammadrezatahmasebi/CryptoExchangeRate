using System.Diagnostics.CodeAnalysis;

namespace Core.CryptoExchangeRate.Domain.Framework;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error is not null)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error is null)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, null);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(null);
}


public class Result<TValue> : Result
{
    
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : default;

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}