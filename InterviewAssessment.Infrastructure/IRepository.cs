using System.Collections.Generic;

namespace InterviewAssessment.Infrastructure
{
    public interface IRepository<out T>
    {
        T Add(string name, double x, double y);
        IEnumerable<T> All();
        void UpdateCoordinate(int id, double x, double y);
    }
}