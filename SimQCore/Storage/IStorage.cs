using SimQCore.Modeller;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimQCore.DataBase
{
    public interface IStorage<T>
        where T : class
    {
        Task<bool> CreateAsync(T problem);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByNameAsync(string name);
        Task<bool> UpdateAsync(T problem);
    }
}