using MedicalAppts.Core.Entities;

namespace MedicalAppts.Core.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<BaseUser?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(BaseUser user);
    }
}
