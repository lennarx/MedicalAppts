using MedicalAppts.Core.Enums;

namespace MedicalAppts.Core.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserStatus UserStatus { get; set; }
        public UserRole UserRole { get; set; }
    }
}
