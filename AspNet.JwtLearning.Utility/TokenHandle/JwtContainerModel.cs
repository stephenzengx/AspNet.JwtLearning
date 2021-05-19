namespace AspNet.JwtLearning.Utility.TokenHandle
{
    public class JwtContainerModel
    {
        //后续可以通过usreid,username 以及rolename进行授权 (放一些前端需要的常用信息)
        public int UserId { get; set; }
        public string UserName { get; set; }       
        public string Guid { get; set; } 
    }
}