namespace PortfolioApp.Models.ResultModels;

public class AppError
{
    public static readonly AppError None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly AppError NullValue = new("General.Null", "Null value was provided", ErrorType.Failure);

    public AppError(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }

    public static AppError Failure(string code, string description) => new(code, description, ErrorType.Failure);
    public static AppError NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
    public static AppError Problem(string code, string description) => new(code, description, ErrorType.Problem);
    public static AppError Conflict(string code, string description) => new(code, description, ErrorType.Conflict);
}
