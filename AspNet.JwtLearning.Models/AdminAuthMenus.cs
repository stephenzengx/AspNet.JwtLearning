using System.Collections.Generic;

namespace AspNet.JwtLearning.Models
{
    public class AdminAuthMenuClass
    {
        public int roleId { get; set; }

        public List<int> selectMenuIds { get; set; }
    }
}
