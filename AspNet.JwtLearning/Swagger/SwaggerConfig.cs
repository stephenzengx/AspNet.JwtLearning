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
    /// swagger ����
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// ע�����
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "JwtLearningDoc");//�ĵ�����
                    c.IncludeXmlComments(GetXmlCommentsPath());//��swagger����xml�ĵ�������
                    c.CustomProvider((defaultProvider) => new SwaggerControllerDescProvider(defaultProvider, GetXmlCommentsPath()));//��ȡ��������ע�ͷ�����
                    c.OperationFilter<SwaggerHeaderFilter>();
                })
                .EnableSwaggerUi(c =>
                {
                    /*
                        None = 0, ��չ��
                        List = 1, ֻչ������
                        Full = 2  չ������
                     */
                    c.DocExpansion((DocExpansion)Enum.Parse(typeof(DocExpansion), ConfigConst.DocExpansionValue));
                    c.DocumentTitle("JwtLearningDoc");
                    c.InjectJavaScript(thisAssembly, "AspNet.JwtLearning.Swagger.SwaggerCustom.js");//����js
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
