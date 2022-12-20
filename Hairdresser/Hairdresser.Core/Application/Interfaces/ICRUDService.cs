using Hairdresser.Core.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Application.Interfaces
{
    public interface ICRUDService<T>
    {
        public FilterResponse<T> GetWithFilter(Filter.Filter filter);
        public T Create(T entity);
        public List<T> Read();
        public T Update(T entity);
        public T Delete(T entity);
        public T Get(int id);
        public T Get(DateTime time);
    }
}
