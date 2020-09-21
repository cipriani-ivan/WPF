using InterviewAssessment.DataAccess;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace InterviewAssessment.Infrastructure
{
    public class EntitiesRepository : IRepository<ExpandoObject>
    {
        private readonly EntitiesDbContext _context;

        public EntitiesRepository(EntitiesDbContext context)
        {
            _context = context;
        }

        public ExpandoObject Add(string name, double x, double y)
        {
            try
            {
                var entityTemp = new entity
                {
                    name = name,
                    coord = new coord()
                    {
                        x = (long)x,
                        y = (long)y
                    }
                };


                _context.entities.Add(entityTemp);


                _context.SaveChanges();

                dynamic entityDynamic = new ExpandoObject();
                entityDynamic.id = entityTemp.id;
                entityDynamic.name = entityTemp.name;
                entityDynamic.x = (double)entityTemp.coord.x;
                entityDynamic.y = (double)entityTemp.coord.y;

                return entityDynamic;
            }
            catch (OverflowException)
            {
                return null;
                //Not ecpected overflow (solution change type of the property in the datbase to contains double
                //TODO: log error and implement a nice error message
            }
        }

        public virtual IEnumerable<ExpandoObject> All()
        {
            var entities = _context.entities.Include("coord").ToList();
            var entitiesDynamic = new List<ExpandoObject>();
            entities.ForEach(
                entity =>
                {
                    entitiesDynamic.Add(MappingEntityToDynamicEntity(entity));
                }
                );
            return entitiesDynamic;

        }

        public void UpdateCoordinate(int id, double x, double y)
        {
            try
            {
                var coord = _context.entities.FirstOrDefault(entity => entity.id == id)?.coord;
                if (coord != null)
                {
                    coord.x = (long)x;
                    coord.y = (long)y;
                }
                _context.SaveChanges();
            }
            catch (OverflowException)
            {
                //Not ecpected overflow (solution change type of the property in the datbase to contains double
                //TODO: log error and implement a nice error message
            }
        }

        private static dynamic MappingEntityToDynamicEntity(entity entityFromEf)
        {
            //TODO: try to use AutoMapper
            var entityDynamic = new ExpandoObject() as IDictionary<string, object>;

            var props = typeof(entity).GetProperties();

            foreach (var prop in props)
            {
                if (prop.Name == "coord")
                {
                    var propsInternal = typeof(coord).GetProperties();
                    foreach (var propInternal in propsInternal)
                    {
                        if (propInternal.Name == "id") continue;
                        if (propInternal.Name == "x" || propInternal.Name == "y")
                        {
                            var test = Convert.ToDouble(propInternal.GetValue(entityFromEf.coord));
                            entityDynamic.Add(propInternal.Name, test);
                        }
                        else
                        {
                            entityDynamic.Add(propInternal.Name, propInternal.GetValue(entityFromEf.coord));
                        }
                    }
                }
                entityDynamic.Add(prop.Name, prop.GetValue(entityFromEf));
            }
            return entityDynamic;
        }
    }
}