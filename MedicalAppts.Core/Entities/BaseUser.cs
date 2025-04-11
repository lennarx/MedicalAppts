using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Entities
{
    public abstract class BaseUser : BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
