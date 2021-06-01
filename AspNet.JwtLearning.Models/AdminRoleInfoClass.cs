namespace AspNet.JwtLearning.Models
{
    public class AdminRoleInfoClass
    {
        public int roleId { get; set; }
        public string roleName { get; set; }
        public int? tenantId { get; set; }
        public string tenantName { get; set; }
    }
}
