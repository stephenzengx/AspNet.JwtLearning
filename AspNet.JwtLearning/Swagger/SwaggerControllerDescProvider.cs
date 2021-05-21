﻿using Swashbuckle.Swagger;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Utils
{
    /// <summary>
    /// swagger显示控制器的描述
    /// </summary>
    public class SwaggerControllerDescProvider : ISwaggerProvider
    {
        private readonly ISwaggerProvider _swaggerProvider;
        private static ConcurrentDictionary<string, SwaggerDocument> _cache = new ConcurrentDictionary<string, SwaggerDocument>();
        private readonly string _xmlPath;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerProvider"></param>
        /// <param name="xml">xml文档路径</param>
        public SwaggerControllerDescProvider(ISwaggerProvider swaggerProvider, string xml)
        {
            _swaggerProvider = swaggerProvider;
            _xmlPath = xml;
        }

        /// <summary>
        /// Gets the swagger.
        /// </summary>
        /// <param name="rootUrl">The root URL.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <returns>SwaggerDocument.</returns>
        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {
            var cacheKey = string.Format("{0}_{1}", rootUrl, apiVersion);
            SwaggerDocument srcDoc;
            //只读取一次
            if (!_cache.TryGetValue(cacheKey, out srcDoc))
            {
                srcDoc = _swaggerProvider.GetSwagger(rootUrl, apiVersion);

                srcDoc.vendorExtensions = new Dictionary<string, object> { { "ControllerDesc", GetControllerDesc() } };
                _cache.TryAdd(cacheKey, srcDoc);
            }
            return srcDoc;
        }

        /// <summary>
        /// 从API文档中读取控制器描述
        /// </summary>
        /// <returns>所有控制器描述</returns>
        public ConcurrentDictionary<string, string> GetControllerDesc()
        {
            string xmlpath = _xmlPath;
            ConcurrentDictionary<string, string> controllerDescDict = new ConcurrentDictionary<string, string>();
            if (File.Exists(xmlpath))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlpath);
                XmlNode summaryNode;

                string type, controllerName;
                string[] arrPath;
                int length, cCount = "Controller".Length;

                // ReSharper disable once PossibleNullReferenceException
                foreach (XmlNode node in xmldoc.SelectNodes("//member"))
                {
                    // ReSharper disable once PossibleNullReferenceException
                    type = node.Attributes["name"].Value;
                    if (type.StartsWith("T:"))
                    {
                        //控制器
                        arrPath = type.Split('.');
                        length = arrPath.Length;
                        controllerName = arrPath[length - 1];
                        if (controllerName.EndsWith("Controller"))
                        {
                            //获取控制器注释
                            summaryNode = node.SelectSingleNode("summary");
                            string key = controllerName.Remove(controllerName.Length - cCount, cCount);
                            if (summaryNode != null && !string.IsNullOrEmpty(summaryNode.InnerText) && !controllerDescDict.ContainsKey(key))
                            {
                                controllerDescDict.TryAdd(key, summaryNode.InnerText.Trim());
                            }
                        }
                    }
                }
            }

            return controllerDescDict;
        }
    }
}