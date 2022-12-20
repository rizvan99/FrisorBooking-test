using Hairdresser.Core.Application.Interfaces;
using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities;
using Hairdresser.Core.Exceptions.Service.General;
using Hairdresser.Core.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Application.Implementation
{
    public class TreatmentService : ICRUDService<Treatment>, IValidator<Treatment>
    {
        public ICrudRepository<Treatment> _repository;
        public TreatmentService(ICrudRepository<Treatment> repository)
        {
            _repository = repository ?? throw new ArgumentException("Repository is missing");
        }
        public void ValidateEntity(Treatment entity)
        {
            if (entity.TreatmentName == null || entity.TreatmentName == "")
            {
                throw new ArgumentException("Navnet på denne service er ikke angivet");
            }
            if (entity.TreatmentDuration == 0 || entity.TreatmentDuration < 0)
            {
                throw new ArgumentException("Tiden på denne service er ikke sat, eller er mindre end 0 minutter");
            }
            if (entity.TreatmentPrice == 0 || entity.TreatmentPrice < 0)
            {
                throw new ArgumentException("Prisen på denne service er ikke sat, eller er mindre end 0 kroner");
            }
            EntityExists(entity);
        }
        public bool EntityExists(Treatment entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Denne service blev ikke oprettet, prøv igen eller kontakt IT-Support");
            }
            return true;
        }
        public Treatment Create(Treatment entity)
        {
            ValidateEntity(entity);
            return _repository.Create(entity);
        }

        public Treatment Delete(Treatment entity)
        {
            if (_repository.Get(entity.TreatmentID) != null)
            {
                return _repository.Delete(entity);
            }
            throw new ArgumentException("Servicen blev ikke fundet");
        }

        public List<Treatment> Read()
        {
            var result = _repository.Read();
            if (result.Count == 0)
            {
                throw new ListIsNullOrEmptyException("Der er ingen services at vise");
            }
            return result;
        }

        public Treatment Update(Treatment entity)
        {
            ValidateEntity(entity);
            if (_repository.Get(entity.TreatmentID) != null)
            {
                return _repository.Update(entity);
            }
            throw new InvalidOperationException("Servicen blev ikke fundet");
        }

        public Treatment Get(int Id)
        {
            return _repository.Get(Id);
        }

        public FilterResponse<Treatment> GetWithFilter(Filter.Filter filter)
        {
            throw new NotImplementedException();
        }

        public Treatment Get(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
