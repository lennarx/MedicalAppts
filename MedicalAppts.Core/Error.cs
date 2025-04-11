namespace MedicalAppts.Core
{
    public sealed record Error(int HttpStatusCode, string? Message = null);
}
