using UnityEditor;

namespace UnityCharts.Editor
{
    [CustomEditor(typeof(PieChart))]
    public class PieChartEditor : UnityEditor.Editor
    {
        private PieChart _target;

        public override void OnInspectorGUI()
        {
            _target = (PieChart) target;

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "m_Material", "m_Color", 
                "m_RaycastTarget", "m_RaycastPadding", "m_Maskable", "m_OnCullStateChanged");

            serializedObject.ApplyModifiedProperties();
        }
    }
}