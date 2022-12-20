using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities;
using Hairdresser.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hairdresser.Infrastructure.Repositories
{
    public class TreatmentRepository : ICrudRepository<Treatment>
    {
        public HairdresserDbContext _ctx { get; set; }
        public TreatmentRepository(HairdresserDbContext ctx)
        {
            _ctx = ctx;
        }

        public Treatment Create(Treatment entity)
        {
            var addedService = _ctx.Treatments.Add(entity).Entity;
            _ctx.SaveChanges();
            return addedService;
        }

        public Treatment Delete(Treatment entity)
        {
            var deletedEntity = _ctx.Remove(entity).Entity;
            _ctx.SaveChanges();
            return deletedEntity;
        }

        public Treatment Get(int Id)
        {
            Treatment foundService = null;
            foreach (Treatment s in _ctx.Treatments)
            {
                if (s.TreatmentID == Id)
                {
                    foundService = s;
                    return foundService;
                }
            }
            return foundService;
        }

        public List<Treatment> Read()
        {
            return _ctx.Treatments.ToList();
        }

        public Treatment Update(Treatment entity)
        {
            var updatedService = _ctx.Treatments.Update(entity).Entity;
            _ctx.SaveChanges();
            return updatedService;
        }

        public Treatment Get(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
