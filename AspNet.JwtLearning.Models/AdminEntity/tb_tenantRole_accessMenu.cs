using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    /// <summary>
    /// 租户角色菜单权限表
    /// </summary>
    public class tb_tenantRole_accessMenu
    {
        //id,tenantId,tenantRoleId,menuId,addTime
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int tenantId { get; set; }
        public int tenantRoleId { get; set; }
        public string menuId { get; set; }
        public DateTime addTime { get; set; }
    }
}
