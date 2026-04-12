using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cinecritic.Application.DTOs.MovieUsers;

namespace Cinecritic.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> CountAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateRangeAsync(IEnumerable<T> entities);
    }
}
