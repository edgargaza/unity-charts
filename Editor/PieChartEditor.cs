using UnityEditor;

namespace UnityCharts.Editor
{
    [CustomEditor(typeof(PieChart))]
    public class PieChartEditor : UnityEditor.Editor
    {
        private new PieChart target;

        private SerializedProperty angleDegreesProperty,
            distanceFromCenterPercentageProperty,
            inlineThicknessProperty,
            inlineColorProperty,
            outlineThicknessProperty,
            outlineColorProperty,
            isAnimatedProperty,
            dataProperty;

        private void OnEnable()
        {
            angleDegreesProperty = serializedObject.FindProperty("angleDegrees");
            distanceFromCenterPercentageProperty = serializedObject.FindProperty("distanceFromCenterPercentage");
            
            inlineThicknessProperty = serializedObject.FindProperty("inlineThickness");
            inlineColorProperty = serializedObject.FindProperty("inlineColor");
            
            outlineThicknessProperty = serializedObject.FindProperty("outlineThickness");
            outlineColorProperty = serializedObject.FindProperty("outlineColor");
            
            isAnimatedProperty = serializedObject.FindProperty("isAnimated");
            
            dataProperty = serializedObject.FindProperty("data");
        }

        public override void OnInspectorGUI()
        {
            target = (PieChart) base.target;

            serializedObject.Update();

            EditorGUILayout.PropertyField(angleDegreesProperty);
            EditorGUILayout.PropertyField(distanceFromCenterPercentageProperty);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(inlineThicknessProperty);
            EditorGUILayout.PropertyField(inlineColorProperty);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(outlineThicknessProperty);
            EditorGUILayout.PropertyField(outlineColorProperty);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isAnimatedProperty);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(dataProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}