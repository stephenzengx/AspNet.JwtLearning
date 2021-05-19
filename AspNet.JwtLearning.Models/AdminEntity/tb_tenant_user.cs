using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    /// <summary>
    /// 租户账号表 (一个租户下有多个账号)
    /// </summary>
    public class tb_tenant_user
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }
        public string userName { get; set; }
        public int tenantId { get; set; }
        public string passWord { get; set; }
        public int age { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool isAllowMultLogin { get; set; }//实现是否允许多端登录  to do
        public DateTime addTime { get; set; }
        public DateTime upateTime { get; set; }
        public bool isEnable { get; set; }
    }
}