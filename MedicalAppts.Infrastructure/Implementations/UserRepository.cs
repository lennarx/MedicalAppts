using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;

namespace MedicalAppts.Infrastructure.Implementations
{
    public class UserRepository : MedicalApptRepository<User>, IUserRepository
    {
        public UserRepository(MedicalApptsDbContext context) : base(context)
        {
        }
    }
}
