using MedicalAppts.Infrastructure;
using MedicalAppts.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MedicalAppts.Test.IntegrationTests.ImplementationsForTest
{
    public class AppointmentTestRepository : AppointmentsRepository
    {
        public AppointmentTestRepository(MedicalApptsDbContext context) : base(context)
        {
        }

        public override async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Task.FromResult(new TestTransaction());
        }

        private class TestTransaction : IDbContextTransaction
        {
            public Guid TransactionId => Guid.Empty;

            public void Commit() { }
            public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

            public void Rollback() { }
            public Task RollbackAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

            public void Dispose() { }
            public ValueTask DisposeAsync() => ValueTask.CompletedTask;

            public string GetDbTransactionId() => Guid.Empty.ToString();
        }
    }
}
