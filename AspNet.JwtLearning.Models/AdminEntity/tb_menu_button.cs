using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    /// <summary>
    /// 页面按钮信息表
    /// </summary>
    public class tb_menu_button
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int btnId { get; set; }
        public int menuId { get; set; }
        public string btnTxt { get; set; }//详情,添加,修改,删除,导入,导出 等
        public int remark { get; set; }      
        public DateTime addTime { get; set; }
        public DateTime updateTime { get; set; }
        public bool isEnable { get; set; }
    }
}
