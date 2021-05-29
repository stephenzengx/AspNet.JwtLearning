using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    public class tb_system_api
    {
        //apiId,apiName,controllerName,actionName,remark,addTime,updateTime,isEnable
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int apiId { get; set; }
        public string apiName { get; set; }
        public string controllerName { get; set; }
        public string actionName { get; set; }
        public string remark { get; set; }
        public DateTime addTime { get; set; }
        public DateTime updateTime { get; set; }
        public bool isEnable { get; set; }
    }
}
