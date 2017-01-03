#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Jerry
{
    /// <summary>
    /// �������
    /// </summary>
    public enum NodeType
    {
        NODEONE,
    }

    /// <summary>
    /// ���
    /// </summary>
    public abstract class NodeBase
    {
        /// <summary>
        /// ���ͣ�������
        /// </summary>
        public NodeType m_NodeType { get; set; }
        public bool m_ShowChildren { get; set; }
        /// <summary>
        /// �ӽ�㣬�������ߵ��յ�
        /// </summary>
        public List<NodeBase> m_Children;
        /// <summary>
        /// ��㴰��100*100
        /// </summary>
        public Rect m_Window = new Rect(10, 100, 100, 100);
        /// <summary>
        /// ����
        /// </summary>
        public NodeContainer m_Container;
        /// <summary>
        /// ����ӽڵ�
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
        /// ���child�Ƿ������Ϊ�ҵ��ӽ��
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public abstract bool CheckChild(NodeBase child);
        public abstract void OnGui();
        /// <summary>
        /// ɾ��ʱ
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
        /// ��������ӽ���������������ıߵ���Ŀ
        /// </summary>
        /// <param name="max"></param>
        public void SetMaxChildren(int max)
        {
            m_MaxChildren = max;
        }

        /// <summary>
        /// ���child�Ƿ���parent������(��)�ڵ�
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