using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    /// <summary>
    /// 系统菜单表
    /// </summary>
    public class tb_system_menu
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int menuId { get; set; }
        public int parentId { get; set; }
        public string menuName { get; set; }
        public int sort { get; set; }
        public string remark { get; set; }
        public DateTime addTime { get; set; }
        public DateTime upateTime { get; set; }
        public bool isEnable { get; set; } //是否禁用
    }
}
