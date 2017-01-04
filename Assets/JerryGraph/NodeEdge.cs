#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

namespace Jerry
{
    /// <summary>
    /// 结点边
    /// </summary>
    public class NodeEdge
    {
        /// <summary>
        /// 起点
        /// </summary>
        public NodeBase m_FromNode { get; set; }
        /// <summary>
        /// 终点
        /// </summary>
        public NodeBase m_ToNode { get; set; }
        public Rect m_StartConnector;
        public Rect m_EndConnector;
        public Vector3 m_EdgePosStart = Vector3.zero;
        public Vector3 m_EdgePosEnd = Vector3.zero;
        /// <summary>
        /// 终点是起点的第几个子节点
        /// </summary>
        private int m_ConnectWidth;
        public static float m_DeleteBtnWidth = 10;
        private float m_CalPosTmp1;
        private float m_CalPosTmp2;

        public NodeEdge(NodeBase from, NodeBase to)
        {
            m_FromNode = from;
            m_ToNode = to;
        }

        public void OnDraw()
        {
            m_StartConnector = new Rect(0, 0, m_DeleteBtnWidth, m_DeleteBtnWidth);
            m_StartConnector.center = m_EdgePosStart + new Vector3(0, m_DeleteBtnWidth / 2, 0);
            
            GUI.Box(m_StartConnector, "");

            //右键删除边
            Event e = Event.current;
            if ((e.type == EventType.MouseDown & e.button == 0) && (m_StartConnector.Contains(e.mousePosition)))
            {
                RemoveEdgeFunction();
            }

            //起点和终点有可能被删除了
            if (m_FromNode != null && m_ToNode != null)
            {
                DrawNodeCurve(m_FromNode.m_Window, m_ToNode.m_Window);
            }
        }

        /// <summary>
        /// 连线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void DrawNodeCurve(Rect start, Rect end)
        {
            m_ConnectWidth = m_FromNode.m_Children.IndexOf(m_ToNode);
            m_CalPosTmp1 = 0;
            m_CalPosTmp2 = start.width / m_FromNode.m_Children.Count;
            if (m_ConnectWidth > 0)
            {
                m_CalPosTmp1 += m_ConnectWidth * m_CalPosTmp2;
            }
            m_CalPosTmp1 += m_CalPosTmp2 / 2;
            Vector3 startPos = new Vector3(start.x + m_CalPosTmp1, start.y + start.height, 0);//LowerCenter
            Vector3 endPos = new Vector3(end.x + end.width / 2, end.y, 0);//UpperCenter
            Vector3 startTan = startPos + Vector3.up * 50;
            Vector3 endTan = endPos + Vector3.down * 50;
            m_EdgePosStart = startPos;
            m_EdgePosEnd = new Vector3(end.x, end.y + end.height / 2, 0);//MiddleCenter

            if (m_FromNode.m_Container.m_SelectNode == m_FromNode)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.green, null, 3);
            }
            else if (m_ToNode.m_Container.m_SelectNode == m_ToNode)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.yellow, null, 3);
            }
            else
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.white, null, 3);
            }
        }

        /// <summary>
        /// 检查结点是否是我的端点，是的话删除这条边
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool CheckForEdge(NodeBase node)
        {
            bool result = false;
            if (m_FromNode == node || m_ToNode == node)
            {
                RemoveEdgeFunction();
                result = true;
            }
            return result;
        }

        private void RemoveEdgeFunction()
        {
            m_FromNode.RemoveChild(m_ToNode);
            m_FromNode.m_Container.RemoveEdge(this);
            m_ConnectWidth = m_FromNode.m_Children.Count;
        }
    }
}
#endif