#if UNITY_EDITOR
using UnityEditor;

namespace Jerry
{
    public class NodeOne : NodeBase
    {
        public NodeOne()
        {
            m_NodeType = NodeType.NODEONE;
            m_MaxChildren = 20;
        }

        public NodeOne(NodeContainer container)
            : base(container)
        {
            m_NodeType = NodeType.NODEONE;
            m_MaxChildren = 20;
        }

        public override void OnGui()
        {
            base.OnGui();
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("NodeOne");
            //EditorGUILayout.EndHorizontal();
        }

        public override bool CheckChild(NodeBase child)
        {
            bool result = false;

            if (CheckForCycles(this, child))
            {
                return false;
            }

            result = true;

            return result;
        }
    }
}
#endif