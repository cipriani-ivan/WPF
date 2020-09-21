using InterviewAssessment.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Windows.Media;

namespace InterviewAssessment.Service
{
    public class EntityStoreService : ObservableCollection<ExpandoObject>
    {
        private readonly IRepository<ExpandoObject> _entityRepository;

        public EntityStoreService(IRepository<ExpandoObject> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public void Load()
        {
            var entities = _entityRepository.All();
            Clear();
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        public void AddEntity(string name, double x, double y)
        {
            var entity = _entityRepository.Add(name, x, y);
            if (entity == null)
            {
                return;
            }
            Add(entity);
        }

        public void UpdateCoordinate(int id, double x, double y, TranslateTransform transform)
        {
            try
            {
                // Inverted x and y for TranslateTransform
                _entityRepository.UpdateCoordinate(id, x + transform.Y, y + transform.X);
            }
            catch (OverflowException)
            {
                //TODO: log error and implement a nice error message
                //Not expected overflow for the screen size
            }
        }
    }
}