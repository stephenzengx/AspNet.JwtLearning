using System.Collections.Generic;

namespace AspNet.JwtLearning.Models
{
    public class AdminAuthClass
    {
        public int roleId { get; set; }

        public List<int> selectIds { get; set; }

        public int menuId { get; set; } //角色授权菜单按钮专用
    }
}
