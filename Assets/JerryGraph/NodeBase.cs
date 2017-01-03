#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Jerry
{
    /// <summary>
    /// 结点类型
    /// </summary>
    public enum NodeType
    {
        NODEONE,
    }

    /// <summary>
    /// 结点
    /// </summary>
    public abstract class NodeBase
    {
        /// <summary>
        /// 类型，做标题
        /// </summary>
        public NodeType m_NodeType { get; set; }
        public bool m_ShowChildren { get; set; }
        /// <summary>
        /// 子结点，和我连边的终点
        /// </summary>
        public List<NodeBase> m_Children;
        /// <summary>
        /// 结点窗口100*100
        /// </summary>
        public Rect m_Window = new Rect(10, 100, 100, 100);
        /// <summary>
        /// 容器
        /// </summary>
        public NodeContainer m_Container;
        /// <summary>
        /// 最大子节点
        /// </summary>
        public int m_MaxChildren { get; set; }
        public Rect m_DeleteButton;

        public NodeBase()
        {
            m_DeleteButton = new Rect(90, 10, 10, 10);
            m_Children = new List<NodeBase>();
            m_Container = null;
        }

        public NodeBase(NodeContainer container)
        {
            m_DeleteButton = new Rect(90, 90, 10, 10);
            m_Children = new List<NodeBase>();
            m_Container = container;
        }

        /// <summary>
        /// 检查child是否可以作为我的子结点
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public abstract bool CheckChild(NodeBase child);
        public abstract void OnGui();
        /// <summary>
        /// 删除时
        /// </summary>
        public virtual void OnDestroy() { }
        
        public void AddChild(NodeBase node)
        {
            m_Children.Add(node);
            m_Window.width = Mathf.Max(100, m_Children.Count * (NodeEdge.m_DeleteBtnWidth + 4));
        }

        public void RemoveChild(NodeBase node)
        {
            m_Children.Remove(node);
            m_Window.width = Mathf.Max(100, m_Children.Count * (NodeEdge.m_DeleteBtnWidth + 4));
        }
        
        public bool GetChildAt(int index, out NodeBase node)
        {
            node = null;
            if (index >= m_Children.Count)
            {
                return false;
            }
            node = m_Children[index];
            return true;
        }

        /// <summary>
        /// 设置最大子结点数，可以连出的边的数目
        /// </summary>
        /// <param name="max"></param>
        public void SetMaxChildren(int max)
        {
            m_MaxChildren = max;
        }

        /// <summary>
        /// 检测child是否是parent结点的子(孙)节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public bool CheckForCycles(NodeBase parent, NodeBase child)
        {
            if (parent.m_Children.Contains(child))
            {
                return true;
            }
            bool result = false;
            foreach (NodeBase node in parent.m_Children)
            {
                if (result = CheckForCycles(node, child))
                {
                    break;
                }
            }
            return result;
        }
    }
}
#endif