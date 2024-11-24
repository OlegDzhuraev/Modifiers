using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Dev
{
	[CustomPropertyDrawer(typeof(ParamGroupAttribute))]
	public class ParamGroupAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!UnityModifiersSettings.TryGetEditor(out var settings))
			{
				GUI.Label(position, "No settings found!");
				return;
			}

			var groups = GetGroups(settings);
			var index = 0;

			for (var i = 0; i < groups.Length; i++)
			{
				var group = groups[i];
				if (group.text == property.stringValue)
					index = i;
			}

			var content = new GUIContent("Group", "You can define custom groups with unique color and other data.");

			var style = new GUIStyle(EditorStyles.popup);

			ModifierAttributeDrawer.DrawGroupIndicator(position, property.stringValue);

			index = EditorGUI.Popup(position, content, index, groups, style);

			if (index >= 0 && index < groups.Length)
				property.stringValue = groups[index].text;
		}

		GUIContent[] GetGroups(UnityModifiersSettings settings)
		{
			var result = new GUIContent[settings.ParamGroups.Count + 1];

			result[0] = new GUIContent("None");

			for (var i = 1; i < settings.ParamGroups.Count + 1; i++)
			{
				var settingsParamGroup = settings.ParamGroups[i - 1];
				result[i] = new GUIContent(settingsParamGroup.Name);
			}

			return result;
		}
	}
}