namespace PortfolioApp.Models.ResultModels;

public class ValidationError : AppError
{
    public ValidationError(AppError[] errors)
        : base("Validation.General", "One or more validation failures have occurred.", ErrorType.Validation)
    {
        Errors = errors;
    }

    public AppError[] Errors { get; }

    public static ValidationError FromResults(IEnumerable<Result> results)
    {
        return new ValidationError([.. results.Select(result => result.Error)]);
    }
}
