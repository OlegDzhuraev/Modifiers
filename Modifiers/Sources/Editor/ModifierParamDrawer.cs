#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Dev
{
	[CustomPropertyDrawer(typeof(ModifierParam))]
	public class ModifierParamDrawer : PropertyDrawer
	{
		const float InputHeight = 20f;
		
		string typePropName = "Type";
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			
			var keyRect = new Rect(position.x, position.y, 150, InputHeight);
			var valueRect = new Rect(position.x + 155, position.y, 110, InputHeight);
			
			EditorGUI.PropertyField(keyRect, property.FindPropertyRelative(typePropName), GUIContent.none);
			EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Value"), GUIContent.none);
			
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var totalHeight = base.GetPropertyHeight(property, label);
			totalHeight += ModifierAttributeDrawer.CalculateExtraHeight(property.FindPropertyRelative(typePropName));

			return totalHeight;
		}
	}
}

#endif