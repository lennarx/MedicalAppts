using MedicalAppts.Core.Contracts.Repositories;
using MedicalAppts.Core.Entities;

namespace MedicalAppts.Infrastructure.Implementations
{
    public class PatientsRepository : MedicalApptRepository<Patient>, IPatientsRepository
    {
        public PatientsRepository(MedicalApptsDbContext context) : base(context)
        {
        }
    }
}
