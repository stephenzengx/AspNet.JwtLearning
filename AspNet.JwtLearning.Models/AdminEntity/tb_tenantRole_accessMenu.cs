﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.JwtLearning.Models.AdminEntity
{
    /// <summary>
    /// 租户角色菜单权限表
    /// </summary>
    public class tb_tenantRole_accessMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int tenantId { get; set; }
        public int tenantRoleId { get; set; }
        public int menuId { get; set; }
        public DateTime addTime { get; set; }
    }
}
