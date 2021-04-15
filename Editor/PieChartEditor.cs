using UnityEditor;

namespace UnityCharts.Editor
{
    [CustomEditor(typeof(PieChart))]
    public class PieChartEditor : UnityEditor.Editor
    {
        private PieChart _target;

        private SerializedProperty _angleDegreesProperty,
            _distanceFromCenterPercentageProperty,
            _inlineThicknessProperty,
            _inlineColorProperty,
            _outlineThicknessProperty,
            _outlineColorProperty,
            _outlineMimicsInlineProperty,
            _isAnimatedProperty,
            _dataProperty;

        private void OnEnable()
        {
            _angleDegreesProperty = serializedObject.FindProperty("angleDegrees");
            _distanceFromCenterPercentageProperty = serializedObject.FindProperty("distanceFromCenterPercentage");
            
            _inlineThicknessProperty = serializedObject.FindProperty("inlineThickness");
            _inlineColorProperty = serializedObject.FindProperty("inlineColor");
            
            _outlineThicknessProperty = serializedObject.FindProperty("outlineThickness");
            _outlineColorProperty = serializedObject.FindProperty("outlineColor");
            _outlineMimicsInlineProperty = serializedObject.FindProperty("outlineMimicsInline");
            
            _isAnimatedProperty = serializedObject.FindProperty("isAnimated");
            
            _dataProperty = serializedObject.FindProperty("data");
        }

        public override void OnInspectorGUI()
        {
            _target = (PieChart) target;

            serializedObject.Update();

            EditorGUILayout.PropertyField(_angleDegreesProperty);
            EditorGUILayout.PropertyField(_distanceFromCenterPercentageProperty);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_inlineThicknessProperty);
            EditorGUILayout.PropertyField(_inlineColorProperty);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_outlineThicknessProperty);
            EditorGUILayout.PropertyField(_outlineColorProperty);
            EditorGUILayout.PropertyField(_outlineMimicsInlineProperty);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_isAnimatedProperty);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_dataProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}