using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Business.Utilities.StatusMessages.Abstract;
using Business.Utilities.StatusMessages.Concrete;
using Business.Utilities.Storage.Abstract;
using Business.Utilities.Storage.Concrete;
using Business.Utilities.Storage.Concrete.Local;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace Business.DependencyResolver.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppDbContext>().SingleInstance();

            builder.RegisterType<TestManager>().As<ITestService>().SingleInstance();
            builder.RegisterType<EFTestDAL>().As<ITestDAL>().SingleInstance();

            builder.RegisterType<GroupManager>().As<IGroupService>().SingleInstance();
            builder.RegisterType<EFGroupDAL>().As<IGroupDAL>().SingleInstance();

            builder.RegisterType<NotificationManager>().As<INotificationService>().SingleInstance();
            builder.RegisterType<EFNotificationDAL>().As<INotificationDAL>().SingleInstance();

            builder.RegisterType<GroupUserManager>().As<IGroupUserService>().SingleInstance();
            builder.RegisterType<EFGroupUserDAL>().As<IGroupUserDAL>().SingleInstance();

            builder.RegisterType<StorageService>().As<IStorageService>().SingleInstance();
            builder.RegisterType<LocalStorage>().As<IStorage>().SingleInstance();

            builder.RegisterType<AuthManager>().As<IAuthService>().SingleInstance();

            builder.RegisterType<RoleManager>().As<IRoleService>().SingleInstance();

            builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }

    }
}
