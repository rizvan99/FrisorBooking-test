using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Application.Interfaces
{
    public interface IValidator<T>
    {
        /// <summary>
        /// Checks if an entity has been instantiated
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean EntityExists(T entity);
        /// <summary>
        /// Validates an entity, checks if it has the correct parameters
        /// </summary>
        /// <param name="entity">the entity you want to validate</param>
        public void ValidateEntity(T entity);
    }
}
