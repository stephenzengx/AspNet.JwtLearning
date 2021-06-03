using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using AspNet.JwtLearning.Models.Tree;

namespace AspNet.JwtLearning.Utility.Common
{
    public class Utils
    {
        /// <summary>
        /// 判断集合是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyList<T>(List<T> list)
        {
            return list == null || list.Count <= 0;
        }

        /// <summary>
        /// 获取当前时间戳 
        /// </summary>
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳</param>
        /// <returns></returns>
        public static string GetTimeStamp(bool bflag=true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return (bflag ? Convert.ToInt64(ts.TotalSeconds).ToString() : Convert.ToInt64(ts.TotalMilliseconds).ToString());
        }

        /// <summary>
        /// 通过节点列表 组织成树
        /// </summary>
        /// <param name="allNode"></param>
        /// <returns></returns>
        public static List<TreeNode> BuildTree(List<Node> allNode)
        {
            if (IsNullOrEmptyList(allNode))
                throw new ArgumentException("empty allNode");

            List<TreeNode> treeNodeList = new List<TreeNode>();

            //遍历所有的根节点
            var rootList = allNode.Where(s => s.ParentId == 0).OrderBy(m => m.Sort).ToList();
            foreach (var ent in rootList)
            {
                TreeNode treeNode = new TreeNode();
                treeNode.NodeId = ent.NodeId;
                treeNode.NodeName = ent.NodeName;
                //treeNode.ParentId = ent.ParentId;
                treeNode.Children = Utility.Common.Utils.GetChildrenTree(ent.NodeId, allNode);
                treeNodeList.Add(treeNode);
            }

            return treeNodeList;
        }

        /// <summary>
        /// 获取某个节点下的节点树
        /// </summary>
        /// <param name="curNodeId"></param>
        /// <param name="allNode"></param>
        /// <returns></returns>
        public static List<TreeNode> GetChildrenTree(int curNodeId, List<Node> allNode)
        {
            if (allNode == null || allNode.Count <= 0)
                return new List<TreeNode>();

            List<TreeNode> TreeList = new List<TreeNode>();
            List<Node> children = allNode.Where(s => s.ParentId == curNodeId).ToList();
            foreach (var ent in children)
            {
                TreeNode treeNode = new TreeNode();
                treeNode.NodeId = ent.NodeId;
                treeNode.NodeName = ent.NodeName;
                //treeNode.ParentId = ent.ParentId;
                treeNode.Children = GetChildrenTree(ent.NodeId, allNode);
                TreeList.Add(treeNode);
            }

            return TreeList;
        }

        /// <summary>
        /// 通过curNodeId 获取自己以及递归下的所有子Node (用于删除功能等)
        /// </summary>
        /// <param name="curNodeId"></param>
        /// <param name="allNode"></param>
        /// <returns></returns>
        public static List<Node> GetRecurNodeList(int curNodeId, List<Node> allNode)
        {
            if (curNodeId <= 0 || allNode == null)
                return null;

            List<Node> list = new List<Node>();
            if (allNode.FirstOrDefault((m => m.NodeId == curNodeId)) != null)
                list.Add(allNode.FirstOrDefault((m => m.NodeId == curNodeId)));
            List<Node> firstLevelNodes = allNode.Where(m => m.ParentId == curNodeId).ToList();
            foreach (var node in firstLevelNodes)
            {
                list.AddRange(GetRecurNodeList(node.NodeId, allNode));
            }

            return list;
        }

        /// <summary>
        /// 获取某个节点的根节点
        /// </summary>
        /// <param name="curNodeId"></param>
        /// <param name="allNode"></param>
        /// <returns></returns>
        public static Node GetRootNode(Node node, List<Node> allNode)
        {
            if (node == null || IsNullOrEmptyList(allNode))
                throw new ArgumentException("GetRootNode Method args error!");

            var curNode = allNode.FirstOrDefault(m => m.NodeId == node.ParentId);
            if (curNode == null)
                return node;

            return GetRootNode(curNode, allNode);
        }

    }
}
