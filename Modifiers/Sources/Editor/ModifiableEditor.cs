/*
 * Copyright 2025 Oleg Dzhuraev <godlikeaurora@gmail.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#if UNITY_EDITOR

using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Dev
{
	[CustomEditor(typeof(Modifiable))]
	public class ModifiableEditor : Editor
	{
		Editor modifierEditor;

		bool showFullModifiersInfo;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var modifiable = target as Modifiable;

			if (!Application.isPlaying && modifiable && modifiable.DefaultModifiers != null)
			{
				foreach (var mod in modifiable.DefaultModifiers)
				{
					if (mod == null)
						continue;

					CreateCachedEditor(mod, null, ref modifierEditor);
					modifierEditor.OnInspectorGUI();
				}
			}

			var runtimeValues = modifiable!.GetAllValuesInternal();

			if (runtimeValues != null)
				DrawModifierValues("Current modifier values", runtimeValues);

			GUILayout.BeginVertical(EditorStyles.helpBox);
			{
				GUILayout.Label("Applied modifiers list", EditorStyles.boldLabel);
				showFullModifiersInfo = GUILayout.Toggle(showFullModifiersInfo, "Show full info");

				var modifiers = modifiable.GetAllModifiersInternal();

				foreach (var mod in modifiers)
				{
					if (showFullModifiersInfo)
					{
						var modValues = mod.Parameters;
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

		void DrawModifierValues(string title, List<ModifierParam> parameters)
		{
			var dict = new Dictionary<string, ModifierParam>();

			foreach (var modifierParam in parameters)
				dict.Add(modifierParam.Type, modifierParam);

			DrawModifierValues(title, dict);
		}

		void DrawModifierValues(string title, Dictionary<string, ModifierParam> parameters)
		{
			UnityModifiersSettings.TryGetEditor(out var settings);

			GUILayout.BeginVertical(EditorStyles.helpBox);
			{
				GUILayout.Label(title, EditorStyles.boldLabel);

				var labelStyle = new GUIStyle(EditorStyles.label);

				var numberStyle = new GUIStyle(EditorStyles.numberField);
				numberStyle.normal.textColor = Color.grey;

				foreach (var (name, param) in parameters)
				{
					GUILayout.BeginHorizontal();
					{
						labelStyle.normal.textColor = ModifierAttributeDrawer.GetColor(name);
						EditorGUILayout.SelectableLabel(name, labelStyle, GUILayout.MaxHeight(20));

						var processed = param.GetProcessedValue(settings);
						var finalText = processed.ToString(CultureInfo.InvariantCulture) + (!Mathf.Approximately(processed, param.Value) ? $" ({param.Value.ToString(CultureInfo.InvariantCulture)})" : "");

						EditorGUILayout.SelectableLabel(finalText, numberStyle, GUILayout.MaxHeight(20));
					}
					GUILayout.EndHorizontal();
				}

			}
			GUILayout.EndVertical();
		}
	}
}

#endif