using InterviewAssessment.Infrastructure;
using InterviewAssessment.Service;
using System.Dynamic;
using System.Windows.Data;
using System.Windows.Media;

namespace InterviewAssessment.ViewModel
{
    public class MainViewModel
    {

        public MainViewModel(IRepository<ExpandoObject> entityRepository)
        {
            EntityStore = new EntityStoreService(entityRepository);

            EntityStore.Load();

            EntitiesBinding = new Binding { Source = EntityStore };
        }

        public Binding EntitiesBinding { get; }
        private EntityStoreService EntityStore { get; }

        public void AddEntity(string name, double x, double y)
        {
            EntityStore.AddEntity(name, x, y);
        }

        public void MoveEntity(int id, double x, double y, TranslateTransform transform)
        {
            EntityStore.UpdateCoordinate(id, x, y, transform);
        }
    }
}