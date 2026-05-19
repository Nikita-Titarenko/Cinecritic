using Cinecritic.Application.Repositories;
using Cinecritic.Infrastructure.Data;

namespace Cinecritic.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        
        public void ClearTracker()
        {
            _context.ChangeTracker.Clear();
        }

        public void Dispose() => _context.Dispose();
    }
}
