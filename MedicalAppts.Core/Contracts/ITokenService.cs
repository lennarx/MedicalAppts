namespace MedicalAppts.Core.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(string userEmail, string role);
    }
}
