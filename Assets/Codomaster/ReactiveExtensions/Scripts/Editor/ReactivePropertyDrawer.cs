using System.Collections;
using System.Reflection;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace Codomaster.ReactiveExtensions.Editor
{
    [CustomPropertyDrawer(typeof(ReactiveProperty<>))]
    public class ReactivePropertyDrawer : PropertyDrawer
    {
        private object _reactiveProperty;
        private MethodInfo _reactivePropertyInvokeChangedEventMethodInfo;
        private WaitForEndOfFrame _waitForEndOfFrame;
        private BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            var valuePropertyHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_value"));
            var changedPropertyHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("Changed"));
    
            return valuePropertyHeight + changedPropertyHeight + EditorGUIUtility.standardVerticalSpacing * 14;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Box(position, GUIContent.none);
            
            var (valProp, valPropRect) = FindValuePropertyData(position, property);
            
            if (IsFoldoutCollapsed(position, property, label))
                return;

            _waitForEndOfFrame = new WaitForEndOfFrame();
            
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.indentLevel += 1;
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(valPropRect, valProp, true);
            if (EditorGUI.EndChangeCheck())
                EditorCoroutineUtility.StartCoroutineOwnerless(WaitOneFrameAndInvokeEventEnumerator());
            
            var (chanProp, chanPropRect) = FindChangedPropertyData(position, property, valProp);
            EditorGUI.PropertyField(chanPropRect, chanProp, new GUIContent(chanProp.name), true);

            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }

        private (SerializedProperty valProp, Rect valPropRect) FindValuePropertyData(Rect position, SerializedProperty property)
        {
            var valProp = property.FindPropertyRelative("_value");
            var targetObject = valProp.serializedObject.targetObject;

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var fieldInfo = targetObject.GetType().GetField(property.propertyPath, bindingFlags);

            _reactiveProperty = fieldInfo.GetValue(targetObject);
            _reactivePropertyInvokeChangedEventMethodInfo = _reactiveProperty.GetType().GetMethod("InvokeChangedEvent", bindingFlags);

            var valuePropertyYPosition = position.y + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
            var valPropRect = new Rect(position.x, valuePropertyYPosition, position.width, EditorGUIUtility.singleLineHeight);

            return (valProp, valPropRect);
        }
        
        private (SerializedProperty chanProp, Rect chanPropRect) FindChangedPropertyData(Rect position, SerializedProperty property, SerializedProperty valProp)
        {
            var chanProp = property.FindPropertyRelative("Changed");
            var changedPropertyYPosition = position.y + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight + 4f + EditorGUI.GetPropertyHeight(valProp);
            var chanPropRect = new Rect(position.x, changedPropertyYPosition, position.width, EditorGUIUtility.singleLineHeight);
            chanPropRect = EditorGUI.IndentedRect(chanPropRect);
            return (chanProp, chanPropRect);
        }
        
        private bool IsFoldoutCollapsed(Rect position, SerializedProperty property, GUIContent label)
        {
            var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            var content = $"{label} ({nameof(ReactiveProperty<int>)}<{_reactiveProperty.GetType().GetGenericArguments()[0].Name}>)";
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, content);

            return !property.isExpanded;
        }

        private IEnumerator WaitOneFrameAndInvokeEventEnumerator()
        {
            yield return _waitForEndOfFrame;
            _reactivePropertyInvokeChangedEventMethodInfo.Invoke(_reactiveProperty, null);
        }
    }
}