using System.Diagnostics.CodeAnalysis;

namespace PortfolioApp.Models.ResultModels;

public class Result
{
    public Result(bool isSuccess, AppError error)
    {
        if (isSuccess && error != AppError.None || !isSuccess && error == AppError.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public AppError Error { get; }

    public static Result Success() => new(true, AppError.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, AppError.None);

    public static Result Failure(AppError error) => new(false, error);

    public static Result<TValue> Failure<TValue>(AppError error) => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, AppError error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of a failure result can't be accessed");

    public static implicit operator Result<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(AppError.NullValue);

    public static Result<TValue> ValidationFailure(AppError error) => new(default, false, error);
}
