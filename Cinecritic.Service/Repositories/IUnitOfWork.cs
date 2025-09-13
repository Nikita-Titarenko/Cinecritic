namespace Cinecritic.Application.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        IMovieRepository Movies { get; }
        Task<int> CommitAsync();
    }
}