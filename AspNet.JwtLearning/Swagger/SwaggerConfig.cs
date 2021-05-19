using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;
using System;
using AspNet.JwtLearning.Swagger;
using Utils;
using AspNet.JwtLearning.Utility;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace AspNet.JwtLearning.Swagger
{
    /// <summary>
    /// swagger 配置
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "JwtLearningDoc");//文档名称
                    c.IncludeXmlComments(GetXmlCommentsPath());//让swagger根据xml文档来解析
                    c.CustomProvider((defaultProvider) => new SwaggerControllerDescProvider(defaultProvider, GetXmlCommentsPath()));//获取控制器的注释方法类
                    c.OperationFilter<SwaggerHeaderFilter>();
                })
                .EnableSwaggerUi(c =>
                {
                    /*
                        None = 0, 不展开
                        List = 1, 只展开操作
                        Full = 2  展开所有
                     */
                    c.DocExpansion((DocExpansion)Enum.Parse(typeof(DocExpansion), ConfigConst.DocExpansionValue));
                    c.DocumentTitle("JwtLearningDoc");
                    c.InjectJavaScript(thisAssembly, "AspNet.JwtLearning.Swagger.SwaggerCustom.js");//汉化js
                });
        }

        /// <summary>
        /// Gets the XML comments path.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string GetXmlCommentsPath()
        {
            return $"{System.AppDomain.CurrentDomain.BaseDirectory}/bin/MyWebapi1.XML";
        }
    }
}
