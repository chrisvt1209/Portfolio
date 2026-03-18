namespace PortfolioApp.Models.ResultModels;

public class ValidationError : Error
{
    public ValidationError(Error[] errors)
        : base("Validation.General", "One or more validation failures have occurred.", ErrorType.Validation)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationError FromResults(IEnumerable<Result> results)
    {
        return new ValidationError([.. results.Select(result => result.Error)]);
    }
}
