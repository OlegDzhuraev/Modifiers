#if UNITY_EDITOR

using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Dev
{
	[CustomEditor(typeof(Modifable))]
	public class ModifableEditor : Editor
	{
		Editor modifierEditor;

		bool showFullModifiersInfo;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var modifable = target as Modifable;

			if (!Application.isPlaying && modifable && modifable.DefaultModifiers != null)
			{
				foreach (var mod in modifable.DefaultModifiers)
				{
					if (mod == null)
						continue;
					
					CreateCachedEditor(mod, null, ref modifierEditor);
					modifierEditor.OnInspectorGUI();
				}
			}

			var primaryValues = modifable.GetAllValuesInternal();

			DrawModifierValues("Current modifier values", primaryValues);

			GUILayout.BeginVertical(EditorStyles.helpBox);
			{
				GUILayout.Label("Applied modifiers list", EditorStyles.boldLabel);
				showFullModifiersInfo = GUILayout.Toggle(showFullModifiersInfo, "Show full info");

				var modifiers = modifable.GetAllModifiersInternal();

				foreach (var mod in modifiers)
				{
					if (showFullModifiersInfo)
					{
						var modValues = mod.GetAllInitializedValues();
						DrawModifierValues(mod.Name, modValues);
					}
					else
					{
						EditorGUILayout.SelectableLabel("- " + mod.Name, EditorStyles.boldLabel, GUILayout.MaxHeight(20));
					}
				}
			}
			GUILayout.EndVertical();
		}

		void DrawModifierValues(string title, Dictionary<string, float> values)
		{
			GUILayout.BeginVertical(EditorStyles.helpBox);
			{
				GUILayout.Label(title, EditorStyles.boldLabel);

				var labelStyle = new GUIStyle(EditorStyles.label);

				var numberStyle = new GUIStyle(EditorStyles.numberField);
				numberStyle.normal.textColor = Color.grey;

				foreach (var (name, value) in values)
				{
					GUILayout.BeginHorizontal();
					{
						labelStyle.normal.textColor = ModifierAttributeDrawer.GetColor(name);
						EditorGUILayout.SelectableLabel(name, labelStyle, GUILayout.MaxHeight(20));
						EditorGUILayout.SelectableLabel(value.ToString(CultureInfo.InvariantCulture), numberStyle,
							GUILayout.MaxHeight(20));
					}
					GUILayout.EndHorizontal();
				}

			}
			GUILayout.EndVertical();
		}
	}
}

#endif