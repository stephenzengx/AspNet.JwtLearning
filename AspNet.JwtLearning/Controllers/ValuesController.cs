using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// values控制器
    /// </summary>
    public class ValuesController : ApiController
    {
        
        /// <summary>
        /// get list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string Get(int id)
        {
            return "get one";
        }

        /// <summary>
        /// post
        /// </summary>
        [HttpPost]
        public string Post()
        {
            return "post";
        }

        /// <summary>
        /// put
        /// </summary>
        [HttpPut]
        public string Put()
        {
            return "put";
        }

        /// <summary>
        /// delelte
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public string Delete()
        {
            return "delete";
        }
    }
}
