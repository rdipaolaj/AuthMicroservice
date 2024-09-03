namespace ssptb.pe.tdlt.auth.dto.User;
public class GetUserByUsernameResponse
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public string SaltPassword { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public string AccountStatus { get; set; } = string.Empty;
}
