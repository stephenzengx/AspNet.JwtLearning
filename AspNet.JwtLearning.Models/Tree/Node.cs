

namespace AspNet.JwtLearning.Models.Tree
{
    public class Node
    {
        public int NodeId { get; set; }      //节点Id
        public int ParentId { get; set; }   //节点父级Id 如果为0则为根节点
        public string Label { get; set; }   //节点名
        public int sort { get; set; } //排序
    }
}
