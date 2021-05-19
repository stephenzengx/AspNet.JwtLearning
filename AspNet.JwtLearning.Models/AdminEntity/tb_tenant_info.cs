using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    /// <summary>
    /// 租户信息表
    /// </summary>
    public class tb_tenant_info
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tenantId { get; set; }
        public string tenantName { get; set; }
        public string address { get; set; }
        public int phone { get; set; }
        public DateTime addTime { get; set; }
        public DateTime upateTime { get; set; }
        public bool isEnable { get; set; }
    }
}
