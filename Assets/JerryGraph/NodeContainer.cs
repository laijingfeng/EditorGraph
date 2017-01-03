#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Jerry
{
    public abstract class NodeContainer : EditorWindow
    {
        public GameObject m_TargetObject { get; private set; }

        /// <summary>
        /// 删除的边
        /// </summary>
        public List<NodeEdge> m_RemoveEdgeList = new List<NodeEdge>();
        /// <summary>
        /// 删除的点
        /// </summary>
        public List<NodeBase> m_RemoveNodeList = new List<NodeBase>();
        /// <summary>
        /// 将要连边的两个结点的索引
        /// </summary>
        public List<int> m_NodesToAttach = new List<int>();
        /// <summary>
        /// 边
        /// </summary>
        public List<NodeEdge> m_NodeEdges = new List<NodeEdge>();
        public NodeType m_SelectedNodeType;

        public NodeContainer()
        {
        }

        public virtual void OnGUI()
        {
        }

        /// <summary>
        /// 删除边
        /// </summary>
        /// <param name="nodeEdge"></param>
        public void RemoveEdge(NodeEdge nodeEdge)
        {
            m_RemoveEdgeList.Add(nodeEdge);
        }

        /// <summary>
        /// 删除结点
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(NodeBase node)
        {
            //删除包含它的边
            foreach (NodeEdge edgeToRemove in m_NodeEdges)
            {
                edgeToRemove.CheckForEdge(node);
            }
            m_RemoveNodeList.Add(node);
        }

        public void DrawEdge(int index)
        {
            m_NodeEdges[index].OnDraw();
        }
    }
}
#endif