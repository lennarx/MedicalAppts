namespace MedicalAppts.Core.Contracts
{
    public interface IJwtService
    {
        string GenerateToken(string userEmail, string role);
    }
}
