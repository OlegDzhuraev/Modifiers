#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Dev
{
	[CustomPropertyDrawer(typeof(ModifierAttribute))]
	public class ModifierAttributeDrawer : PropertyDrawer
	{
		static readonly float helpHeight = 18f;
		static readonly float separator = 6f;
		static readonly float groupIndicatorWidth = 3f;

		static readonly List<string> textsCache = new List<string>();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			
			var isCorrect = IsCorrectId(property.stringValue);
			var prevColor = GUI.color;

			if (!isCorrect)
				GUI.color = Color.yellow;

			var propertyRect = new Rect(position.x, position.y, position.width, 20f);

			var groupColor = GetColor(property.stringValue);

			property.stringValue = EditorGUI.TextField(propertyRect, property.stringValue);

			if (groupColor != Color.white)
			{
				var groupRect = new Rect(propertyRect.x - groupIndicatorWidth, propertyRect.y, groupIndicatorWidth, propertyRect.height);
				EditorGUI.DrawRect(groupRect, groupColor);
			}

			GUI.color = prevColor;

			if (!isCorrect)
			{
				var helpText = GetSimilarText(property.stringValue);
				var helpRect = new Rect(propertyRect.x + 1, propertyRect.y, propertyRect.width, 20f);
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				
				EditorGUI.LabelField(helpRect, helpText);
				GUI.color = prevColor;
				
				var all = GetAllSimilarText(property.stringValue);

				for (var i = 0; i < all.Count; i++)
				{
					var s = all[i];
					var helpRectBig = new Rect(position.x + 3, position.y + separator + helpHeight * (i + 1),
						position.width, helpHeight);

					var color = i % 2 == 0 ? new Color(0.1f, 0.1f, 0.1f, 1f) : new Color(0.125f, 0.125f, 0.125f, 1f);
					EditorGUI.DrawRect(helpRectBig, color);

					if (GUI.Button(helpRectBig, s, EditorStyles.label))
					{
						property.stringValue = s;
						GUI.FocusControl(null);
					}
				}
			}
		}

		public static void DrawParamGroupIndicator(Rect baseRect, string param)
		{
			var groupColor = GetColor(param);
			DrawGroupIndicator(baseRect, groupColor);
		}

		public static void DrawGroupIndicator(Rect baseRect, string groupName)
		{
			var groupColor = GetGroupColor(groupName);
			DrawGroupIndicator(baseRect, groupColor);
		}

		public static void DrawGroupIndicator(Rect baseRect, Color color)
		{
			if (color != Color.white)
			{
				var groupRect = new Rect(baseRect.x - groupIndicatorWidth, baseRect.y, groupIndicatorWidth, baseRect.height);
				EditorGUI.DrawRect(groupRect, color);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var baseHeight = base.GetPropertyHeight(property, label);
			
			return baseHeight + CalculateExtraHeight(property);
		}

		public static float CalculateExtraHeight(SerializedProperty property)
		{
			if (IsCorrectId(property.stringValue))
				return 0;
			
			var all = GetAllSimilarText(property.stringValue);
			var helpAmount = all.Count;

			if (helpAmount == 0)
				return 0;
			
			return separator + helpAmount * helpHeight;
		}

		public static bool IsCorrectId(string textId)
		{
			if (!UnityModifiersSettings.TryGetEditor(out var defaultMod))
				return false;
			
			var mods = defaultMod.SupportedParams;
			for (var i = 0; i < mods.Length; i++)
				if (mods[i].Name == textId)
					return true;

			return false;
		}

		public static string GetSimilarText(string startsWith)
		{
			if (startsWith == "" || !UnityModifiersSettings.TryGetEditor(out var defaultMod))
				return "";
			
			var mods = UnityModifiersSettings.Get().SupportedParams;
			
			for (var i = 0; i < mods.Length; i++)
				if (mods[i].Name == startsWith)
					return mods[i].Name;

			for (var i = 0; i < mods.Length; i++)
				if (mods[i].Name.StartsWith(startsWith))
					return mods[i].Name;

			return "";
		}
		
		public static List<string> GetAllSimilarText(string startsWith)
		{
			textsCache.Clear();

			if (startsWith == "" || !UnityModifiersSettings.TryGetEditor(out var defaultMod))
				return textsCache;
			
			var mods = defaultMod.SupportedParams;
			
			for (var i = 0; i < mods.Length; i++)
				if (mods[i].Name.Contains(startsWith))
					textsCache.Add(mods[i].Name);
			
			return textsCache;
		}

		public static Color GetColor(string modifierName)
			=> !UnityModifiersSettings.TryGetEditor(out var defaultMod) ? Color.white : defaultMod.GetEditorColor(modifierName);

		public static Color GetGroupColor(string groupName)
			=> !UnityModifiersSettings.TryGetEditor(out var defaultMod) ? Color.white : defaultMod.GetEditorGroupColor(groupName);
	}
}

#endif