using UnityEngine;
using Jerry;
using UnityEditor;

public class JerryTest : MonoBehaviour
{
    public ConditionContainer m_ConditionContainer { get; private set; }

    [ContextMenu("ShowWin")]
    private void ShowWin()
    {
        m_ConditionContainer = EditorWindow.GetWindow<ConditionContainer>("Condition", true);
    }
}