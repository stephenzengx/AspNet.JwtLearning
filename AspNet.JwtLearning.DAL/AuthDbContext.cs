using System.Data.Entity;

namespace AspNet.JwtLearning.DAL
{
    using AspNet.JwtLearning.Models.AdminEntity;
    /*
        迁移脚本：
         Install-Package EntityFramework
         Enable-Migrations -EnableAutomaticMigrations 
         Add-Migration InitialCreatexxx
         Update-Database -Verbose

        更新了实体：
        Add-Migration udpv1
        Update-Database -Verbose

        //联合主键
        [Key,Column(Order =1)]
        public int Id1 { get; set; }
        [Key,Column(Order = 2)]
        public int Id2 { get; set; }
    */


    //No MigrationSqlGenerator found for provider 'MySql.Data.MySqlClient'.
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class AuthDbContext : DbContext
    {
        public AuthDbContext() : base("name=dbauthtest")
        {
            //初始化EF策略(取消当数据库模型发生改变时删除当前数据库重建新数据库的设置。)
            Database.SetInitializer<AuthDbContext>(null);
            // 禁用延迟加载
            //this.Configuration.LazyLoadingEnabled = false;
            //Database.SetInitializer(new DbAuthTestInit());//关闭数据库初始化操作：
        }

        public DbSet<tb_tenant_info> tb_tenant_infos { get; set; }
        public DbSet<tb_roleInfo> tb_roleInfos { get; set; }

        public DbSet<tb_user> tb_users { get; set; }
        public DbSet<tb_user_role> tb_user_roles { get; set; }

        public DbSet<tb_system_menu> tb_system_menus { get; set; }
        public DbSet<tb_role_accessMenu> tb_role_accessMenus { get; set; }

        public DbSet<tb_system_api> tb_system_apis { get; set; }
        public DbSet<tb_role_accessApi> tb_role_accessApis { get; set; }

        public DbSet<tb_menu_button> tb_menu_buttons { get; set; }
        public DbSet<tb_role_accessBtn> tb_role_Accessbtns { get; set; }

        //fluent api
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { }
    }
}