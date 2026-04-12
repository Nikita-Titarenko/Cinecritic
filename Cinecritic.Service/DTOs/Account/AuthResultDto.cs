namespace Cinecritic.Application.DTOs.Account
{
    public class AuthResultDto
    {
        public Guid? UserId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
