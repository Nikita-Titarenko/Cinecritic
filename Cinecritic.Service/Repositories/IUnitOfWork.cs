namespace Cinecritic.Application.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
        void ClearTracker();
    }
}