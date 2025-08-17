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
using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace InsaneOne.Modifiers.Tools
{
	public class UnityCsvWindow : EditorWindow
	{
		ObjectField exportPresetField;
		TextField importPathField;
		VisualElement exportButtonsRow;

		[MenuItem("Tools/InsaneOne Modifiers/CSV Tools...")]
		public static void Init()
		{
			var window = (UnityCsvWindow)GetWindow(typeof(UnityCsvWindow));
			window.titleContent = new GUIContent("Modifiers CSV Tools");
			window.Show();
		}

		void CreateGUI()
		{
			var root = rootVisualElement;

			var exportLbl = new Label("Export")
			{
				style =
				{
					fontSize = 14,
					unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
					paddingBottom = 6, paddingTop = 6, paddingLeft = 4,
				},
			};

			exportPresetField = new ObjectField { objectType = typeof(CsvExportPreset) };
			exportPresetField.RegisterCallback<ChangeEvent<Object>>(evt =>
			{
				var preset = evt.newValue as CsvExportPreset;
				exportButtonsRow.SetEnabled(preset != null);
			});

			var exportConsoleBtn = new Button(OnExportConsoleClick) { text = "Export CSV to console" };
			var exportFileBtn = new Button(OnExportFileClick) { text = "Export CSV to file in Assets" };

			exportButtonsRow = new VisualElement
			{
				style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row) },
			};
			exportButtonsRow.Add(exportConsoleBtn);
			exportButtonsRow.Add(exportFileBtn);
			exportButtonsRow.SetEnabled(exportPresetField.value is CsvExportPreset);

			var importLbl = new Label("Import")
			{
				style =
				{
					fontSize = 14,
					unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
					paddingBottom = 6, paddingTop = 6, paddingLeft = 4,
				},
			};

			var importPathRow = new VisualElement
			{
				style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row) },
			};

			importPathField = new TextField("Import from path") { style = { flexGrow = 1 } };
			var browseBtn = new Button(OnBrowseClick) { text = "Browse..." };

			importPathRow.Add(importPathField);
			importPathRow.Add(browseBtn);

			var importBtn = new Button(OnImportClick) { text = "Import CSV to exist assets" };

			root.Add(exportLbl);
			root.Add(exportPresetField);
			root.Add(exportButtonsRow);
			root.Add(importLbl);
			root.Add(importPathRow);
			root.Add(importBtn);
		}

		void OnBrowseClick()
		{
			var path = EditorUtility.OpenFilePanel("Select a file", Application.dataPath, "csv");
			if (!string.IsNullOrEmpty(path))
				importPathField.value = path;
		}

		void OnExportConsoleClick()
		{
			var settings = (exportPresetField.value as CsvExportPreset)!.GetExportSettings();
			var result = CsvSerialization.Serialize(settings);
			Debug.Log(result);
		}

		void OnExportFileClick()
		{
			try
			{
				var settings = (exportPresetField.value as CsvExportPreset)!.GetExportSettings();
				var path = Path.Combine(Application.dataPath, "ModifiersExport_rid_" + Random.Range(0, 9999) + ".csv");
				var result = CsvSerialization.Serialize(settings);

				File.WriteAllText(path, result);
				importPathField.value = path;

				Debug.Log($"Successfully exported modifiers CSV at path: {path}");
			}
			catch (Exception e)
			{
				Debug.LogError("Failed to export modifiers. Error: " + e);
			}
		}

		void OnImportClick()
		{
			try
			{
				var csvString = File.ReadAllText(importPathField.value);
				Import(csvString);
			}
			catch (Exception e)
			{
				Debug.LogError("Failed to import modifiers. Error: " + e);
			}
		}

		void Import(string csvString)
		{
			var importedAmount = 0;
			var importedNames = "";
			var modifiers = CsvSerialization.Deserialize(csvString);

			foreach (var mod in modifiers)
			{
				var suitableAssetGuids = AssetDatabase.FindAssets($"{mod.Name} t:{nameof(UnityModifier)}");

				foreach (var guid in suitableAssetGuids)
				{
					var path = AssetDatabase.GUIDToAssetPath(guid);
					var unityMod = AssetDatabase.LoadAssetAtPath<UnityModifier>(path);

					unityMod.EditorSetModifier(mod);

					importedNames += unityMod.name + "\n";
					importedAmount++;
				}
			}

			Debug.Log($"Imported {importedAmount} modifiers:\n{importedNames}");
		}
	}
}
#endif