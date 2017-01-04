using UnityEngine;
using Jerry;
using UnityEditor;

public class JerryTest : MonoBehaviour
{
    public TestContainer m_TestContainer { get; private set; }

    [ContextMenu("ShowWin")]
    private void ShowWin()
    {
        m_TestContainer = EditorWindow.GetWindow<TestContainer>("Test", true);
    }
}