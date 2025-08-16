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
using UnityEngine;
using Random = UnityEngine.Random;

namespace InsaneOne.Modifiers.Tools
{
	public class UnityCsvWindow : EditorWindow
	{
		[SerializeField] UnityModifier[] modifiers;

		string importPath;
		SerializedObject so;

		[MenuItem("Tools/InsaneOne Modifiers/CSV Tools...")]
		public static void Init()
		{
			var window = (UnityCsvWindow)GetWindow(typeof(UnityCsvWindow));
			window.titleContent = new GUIContent("Modifiers CSV Tools");
			window.Show();
		}

		void OnEnable()
		{
			ScriptableObject target = this;
			so = new SerializedObject(target);
		}
		
		void OnGUI()
		{
			so.Update();
			var serialProp = so.FindProperty("modifiers");

			GUILayout.Label("Export", EditorStyles.whiteLargeLabel);
			EditorGUILayout.PropertyField(serialProp, true);
			so.ApplyModifiedProperties();
			
			var prevEnabled = GUI.enabled;
			GUI.enabled = modifiers is {Length: > 0};

			if (GUILayout.Button("Export CSV to console"))
				Debug.Log(MakeExportString(modifiers));
			
			if (GUILayout.Button("Export CSV to file in Assets"))
			{
				try
				{
					var path = Path.Combine(Application.dataPath, "ModifiersExport_rid_" + Random.Range(0, 9999) + ".csv");
					File.WriteAllText(path, MakeExportString(modifiers));
					importPath = path;

					Debug.Log($"Successfully exported modifiers CSV at path: {path}");
				}
				catch (Exception e)
				{
					Debug.LogError("Failed to export modifiers. Error: " + e);
				}
			}

			GUI.enabled = prevEnabled;

			GUILayout.Label("Import", EditorStyles.whiteLargeLabel);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Import from path");
				importPath = GUILayout.TextField(importPath);
			}

			if (GUILayout.Button("Browse..."))
			{
				var path = EditorUtility.OpenFilePanel("Select a file", Application.dataPath, "csv");
				if (!string.IsNullOrEmpty(path))
					importPath = path;
			}
			GUILayout.EndHorizontal();

			if (GUILayout.Button("Import CSV to exist assets"))
			{
				try
				{
					var csvString = File.ReadAllText(importPath);
					Import(csvString);
				}
				catch (Exception e)
				{
					Debug.LogError("Failed to import modifiers. Error: " + e);
				}
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

		static string MakeExportString(UnityModifier[] mods)
		{
			var rawModifiers = new Modifier[mods.Length];
			for (var i = 0; i < rawModifiers.Length; i++)
				rawModifiers[i] = mods[i].Modifier;
				
			return CsvSerialization.Serialize(rawModifiers);
		}
	}
}
#endif