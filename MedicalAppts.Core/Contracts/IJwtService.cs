namespace MedicalAppts.Core.Contracts
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string role);
    }
}
