using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace AspNet.JwtLearning.Utility.Common
{
    public static class Utils
    {
        /// <summary>
        /// 获取当前时间戳 
        /// </summary>
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳</param>
        /// <returns></returns>
        public static string GetTimeStamp(bool bflag=true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return (bflag ? Convert.ToInt64(ts.TotalSeconds).ToString() : Convert.ToInt64(ts.TotalMilliseconds).ToString());
        }

        /// <summary>  
        /// DataTable 转换为List 集合(扩展方法)  
        /// </summary>  
        /// <param name="dt">DataTable</param>  
        /// <returns></returns>  
        public static List<T> ToList<T>(this DataTable dt)
        {
            //创建一个属性的列表  
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口  
            Type t = typeof(T);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表   
            Array.ForEach(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合  
            List<T> obList = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                T ob = Activator.CreateInstance<T>();
                //T ob = new T();
                //找到对应的数据  并赋值  
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.  
                obList.Add(ob);
            }

            return obList;
        }
    }
}
