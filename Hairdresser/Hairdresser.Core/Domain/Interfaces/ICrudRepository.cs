using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Domain.Interfaces
{
    public interface ICrudRepository<T>
    {
        public T Create(T entity);
        public List<T> Read();
        public T Update(T entity);
        public T Delete(T entity);
        public T Get(int id);
    }
}
