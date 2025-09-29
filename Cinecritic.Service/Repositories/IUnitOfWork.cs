namespace Cinecritic.Application.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        IMovieRepository Movies { get; }
        IMovieUserRepository MovieUsers { get; }
        IReviewRepository Reviews { get; }
        IWatchListRepository WatchLists { get; }

        Task<int> CommitAsync();
    }
}