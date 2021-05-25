using System.Collections.Generic;

namespace AspNet.JwtLearning.Models.Tree
{
    public class TreeNode
    {
        public int NodeId { get; set; }      //节点Id
        //public int ParentId { get; set; }   //节点父级Id 如果为0则为根节点
        public string MenuName { get; set; }   //节点名
        public List<TreeNode> Children { get; set; }//子节点树
    }
}
