using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    /// <summary>
    /// 租户角色-接口权限表
    /// </summary>
    public class tb_role_accessApi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        //public int tenantId { get; set; }
        public int roleId { get; set; }
        public int apiId { get; set; }
        public DateTime addTime { get; set; }
    }
}
