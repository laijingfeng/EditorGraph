#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Jerry
{
	public class TestContainer : NodeContainer
	{
        /// <summary>
        /// 结点
        /// </summary>
		public List<NodeBase> m_ConditionList { get; set; }

		Rect canvas = new Rect(0, 0, 2000, 2000);
		Vector2 scrollPosition = Vector2.zero;
        /// <summary>
        /// 窗口最大X坐标
        /// </summary>
		float maxXPosition = 0;
        /// <summary>
        /// 窗口最大Y坐标
        /// </summary>
		float maxYPosition = 0;

        TestContainer()
		{
			m_ConditionList = new List<NodeBase>();
			minSize = new Vector2(800.0f, 500.0f);
		}

		public override void OnGUI()
		{
			base.OnGUI();

			GUILayout.Label("Condition Inspector", EditorStyles.boldLabel);
			
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Add Node: ",GUILayout.ExpandWidth(false));

            if (GUILayout.Button("NodeOne", GUILayout.ExpandWidth(false)))
            {
                m_ConditionList.Add(new NodeOne(this));
            }

			if (m_NodesToAttach.Count == 1) 
			{
				if(GUILayout.Button("Cancel", GUILayout.ExpandWidth (false)))
				{
					m_NodesToAttach.Clear();
				}
			}

			GUILayout.EndHorizontal();
			
			if (m_NodesToAttach.Count == 2) 
			{
				if (m_NodesToAttach[0] != m_NodesToAttach[1])
				{
					m_ConditionList[m_NodesToAttach[0]].AddChild(m_ConditionList[m_NodesToAttach[1]]);
					m_NodeEdges.Add(new NodeEdge(m_ConditionList[m_NodesToAttach[0]], m_ConditionList[m_NodesToAttach[1]]));
					m_NodesToAttach.Clear();
				}
			}

			Event e = Event.current;
			//Canvas Scroll pan
            if (e.button == 0 && e.isMouse && e.type == EventType.MouseDrag && e.alt)
            {
                scrollPosition += e.delta * 2;
            }
			
			//find the max windows position
			for (int i = 0; i < m_ConditionList.Count; i++)
			{
				maxXPosition = Mathf.Max (m_ConditionList[i].m_Window.xMax, maxXPosition);
				maxYPosition = Mathf.Max (m_ConditionList[i].m_Window.yMax, maxYPosition);
			}
			
			//Get and set canvas limits for the nodes
			Vector2 canvasLimits = new Vector2(Mathf.Max(500, maxXPosition), Mathf.Max(100, maxYPosition));
			canvas.width = canvasLimits.x;
			canvas.height = canvasLimits.y;
			
			Rect actualCanvas = new Rect(5, 0, position.width - 10, position.height);
			
			scrollPosition = GUI.BeginScrollView (actualCanvas, scrollPosition, canvas);

			BeginWindows();

			foreach (NodeBase node in m_ConditionList)
			{
				GUILayoutOption[] options = 
                { 
                    GUILayout.ExpandHeight(true), 
                    GUILayout.ExpandWidth(true), 
                    GUILayout.MinHeight(100f), 
                    GUILayout.MinWidth(100f) 
                };
				int index = m_ConditionList.IndexOf(node);
				node.m_Window = GUILayout.Window(index, node.m_Window, DrawNode, node.m_NodeType.ToString(), options);
			}

			foreach (NodeEdge edge in m_NodeEdges)
			{
				edge.OnDraw();
			}

			foreach (NodeEdge deletedEdge in m_RemoveEdgeList)
			{
				m_NodeEdges.Remove(deletedEdge);
			}

			foreach (NodeBase deletedNode in m_RemoveNodeList)
			{
				m_ConditionList.Remove(deletedNode);
				deletedNode.OnDestroy();
			}

			m_RemoveEdgeList.Clear();
			m_RemoveNodeList.Clear();

			EndWindows();
			GUI.EndScrollView();
		}

        /// <summary>
        /// 绘制结点
        /// </summary>
        /// <param name="index"></param>
		void DrawNode(int index)
		{
			m_ConditionList[index].OnGui();

            if (m_ConditionList[index].m_Children.Count < m_ConditionList[index].m_MaxChildren && (m_NodesToAttach.Count != 1))
            {
                if (GUILayout.Button("Make Parent"))
                {
                    m_NodesToAttach.Add(index);
                }
            }

            if ((m_NodesToAttach.Count == 1) && (index != m_NodesToAttach[0]) && m_ConditionList[m_NodesToAttach[0]].CheckChild(m_ConditionList[index]))
            {
                if (GUILayout.Button("Make Child"))
                {
                    m_NodesToAttach.Add(index);
                }
            }

			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			Color currentColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.red;
			GUILayoutOption[] options = 
            { 
                GUILayout.ExpandHeight(false), 
                GUILayout.ExpandWidth(false), 
                GUILayout.Height(15f), 
                GUILayout.Width(15f) 
            };

            //删除结点，删除它保存的子结点，删除它自己
			if (GUILayout.Button(" ", options))
			{
                for (int i = 0, imax = m_ConditionList[index].m_Children.Count; i < imax; i++)
                {
                    NodeBase child = m_ConditionList[i];
                    if (child != null)
                    {
                        m_ConditionList[index].RemoveChild(child);
                    }
                }

				RemoveNode(m_ConditionList[index]);
			}
			
			GUI.backgroundColor = currentColor;
			GUILayout.EndHorizontal();
			GUI.DragWindow();
		}
	}
}
#endif