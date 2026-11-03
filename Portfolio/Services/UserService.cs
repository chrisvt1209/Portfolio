namespace Portfolio.Services;

public class UserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsername()
    {
        throw new NotImplementedException();
    }

    public string GetRemoteIpAddress()
    {
        var remoteIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;
        return remoteIpAddress?.ToString() ?? string.Empty;
    }
}
