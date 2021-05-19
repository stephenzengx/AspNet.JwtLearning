using AspNet.JwtLearning.Helpers;
using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Reflection;
using System.Web.Http;

namespace AspNet.JwtLearning.App_Start
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            try
            {
                //builder.RegisterControllers(Assembly.GetCallingAssembly());  //注册mvc控制器  需要引用package Autofac.Mvc
                //builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();  //支持Api控制器属性注入
                builder.RegisterApiControllers(Assembly.GetCallingAssembly());  //注册所有api控制器  构造函数注入  需要引用package Autofac.WebApi

                //注册 BLL
                var assemblysServices = Assembly.Load("AspNet.JwtLearning.BLL");
                builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerDependency();

                //注册 DAL
                var assemblysRepository = Assembly.Load("AspNet.JwtLearning.DAL");
                builder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces()

                //.SingleInstance();
                //.InstancePerLifetimeScope();

                .InstancePerDependency();

                //注册 DbContext 为scope, to do

                _container = builder.Build();   //创建依赖注入

                //设置MVC依赖注入
                //DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));

                //设置WebApi依赖注入
                GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog((ex.InnerException!=null?ex.InnerException.Message:ex.Message) + ex.StackTrace);
            }
        }

        /// <summary>
        /// 从Autofac容器获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFromFac<T>()
        {
            return _container.Resolve<T>();
        }

    }
}