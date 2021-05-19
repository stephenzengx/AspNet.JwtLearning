using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    /// <summary>
    /// 系统角色信息表 (每个子系统都有各自单独的角色)
    /// </summary>
    public class tb_system_roleInfo
    {        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int roleId { get; set; }
        public string roleName { get; set; }
        public int tenantId { get; set; }
        public int roleType { get; set; }//超级管理员，系统管理员，普通用户等
        public string dutyRemark { get; set; }
        public DateTime addTime { get; set; }
        public DateTime upateTime { get; set; }
        public bool isEnable { get; set; }
    }
}
