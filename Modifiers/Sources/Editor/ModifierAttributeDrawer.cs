#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Dev
{
	[CustomPropertyDrawer(typeof(ModifierAttribute))]
	public class ModifierAttributeDrawer : PropertyDrawer
	{
		static float helpHeight = 18f;
		static float separator = 6f;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			
			var isCorrect = IsCorrectId(property.stringValue);
			var prevColor = GUI.color;

			if (!isCorrect)
				GUI.color = Color.yellow;

			var propertyRect = new Rect(position.x, position.y, position.width, 20f);
			property.stringValue = EditorGUI.TextField(propertyRect, property.stringValue);

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
					EditorGUI.LabelField(helpRectBig, s);
				}
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
			var defaultMod = DefaultUnityModifierSettings.Get();

			if (!defaultMod)
				return false;
			
			var mods = defaultMod.SupportedModifiers;
			for (var i = 0; i < mods.Length; i++)
				if (mods[i] == textId)
					return true;

			return false;
		}

		public static string GetSimilarText(string startsWith)
		{
			var defaultMod = DefaultUnityModifierSettings.Get();
			
			if (startsWith == "" || !defaultMod)
				return "";
			
			var mods = DefaultUnityModifierSettings.Get().SupportedModifiers;
			
			for (var i = 0; i < mods.Length; i++)
				if (mods[i] == startsWith)
					return mods[i];

			for (var i = 0; i < mods.Length; i++)
				if (mods[i].StartsWith(startsWith))
					return mods[i];

			return "";
		}
		
		public static List<string> GetAllSimilarText(string startsWith)
		{
			var defaultMod = DefaultUnityModifierSettings.Get();
			var results = new List<string>();
			
			if (startsWith == "" || !defaultMod)
				return results; 
			
			var mods = defaultMod.SupportedModifiers;
			
			for (var i = 0; i < mods.Length; i++)
				if (mods[i].Contains(startsWith))
					results.Add(mods[i]);
			
			return results;
		}
	}
}

#endif