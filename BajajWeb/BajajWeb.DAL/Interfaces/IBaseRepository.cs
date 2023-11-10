using System.Collections.Generic;

namespace BajajWeb.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        List<T> GetAll();

        T Update(T entity);

        void Register(T entity);
    }
}
