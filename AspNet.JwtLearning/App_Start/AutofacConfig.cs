using AspNet.JwtLearning.DAL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Utility.Log;
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
                //注册所有api控制器  
                builder.RegisterApiControllers(Assembly.GetExecutingAssembly()); 

                //注册 BLL
                builder.RegisterAssemblyTypes(Assembly.Load("AspNet.JwtLearning.BLL"));

                //注册 DAL
                builder.RegisterAssemblyTypes(Assembly.Load("AspNet.JwtLearning.DAL"));
                //.AsImplementedInterfaces();
                //.InstancePerDependency();
                //.SingleInstance();
                //.InstancePerLifetimeScope();

                builder.RegisterType<AuthDbContext>().InstancePerRequest();

                //编译一下
                _container = builder.Build();  
                //webapi整个的解析依赖,交给aufofac去解析 
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
        public static T GetFromAutoFac<T>()
        {
            return _container.Resolve<T>();
        }

    }
}