using Autofac;
using InterviewAssessment.DataAccess;
using InterviewAssessment.Infrastructure;
using InterviewAssessment.ViewModel;
using System.Dynamic;

namespace InterviewAssessment.StartUp
{
    public class Bootstrapper
    {
        public static IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EntitiesDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<EntitiesRepository>().As<IRepository<ExpandoObject>>();
            builder.RegisterType<ExpandoObject>().AsSelf();
            return builder.Build();
        }
    }
}