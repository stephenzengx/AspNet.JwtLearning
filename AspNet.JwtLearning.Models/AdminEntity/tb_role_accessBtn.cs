using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    /// <summary>
    /// 角色(按钮)操作权限表
    /// </summary>
    public class tb_role_accessBtn
    {
        //id,roleId,tenantId,,menuId,btnId,addTime
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int roleId { get; set; }
        //public int tenantId { get; set; }
        public int menuId { get; set; }
        public int btnId { get; set; }
        public DateTime addTime { get; set; }
    }
}
