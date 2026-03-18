namespace PortfolioApp.Models.ResultModels.Errors;

public static class ProjectErrors
{
    public static Error NotFound(int projectId)
    {
        return Error.NotFound("Projects.NotFound", $"The project with id '{projectId}' was not found");
    }

    public static readonly Error NameNotUnique = Error.Conflict("Projects.NameNotUnique", "The provided name is already in use");

    public static readonly Error InvalidProject = Error.Failure("Projects.InvalidProject", "The project must be valid");
}
