﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinecritic.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(params object[] keyValues);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
