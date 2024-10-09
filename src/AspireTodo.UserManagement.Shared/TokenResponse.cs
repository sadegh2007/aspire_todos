namespace AspireTodo.UserManagement.Shared;

public class TokenResponse
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public string RefreshToken { get; set; }
    public long Expires { get; set; }
    public UserDto User { get; set; }
}