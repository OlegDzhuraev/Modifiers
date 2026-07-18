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

#if UNITY_5_3_OR_NEWER
using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Dev
{
	public class ModifiersInitializeWindow : EditorWindow
	{
		public const string DismissedSessionKey = "InsaneOne.Modifiers.Setup.Dismissed";

		[MenuItem("Tools/InsaneOne Modifiers/Initial setup...", priority = 0)]
		public static void ShowWindow()
		{
			var wnd = CreateInstance<ModifiersInitializeWindow>();
			wnd.titleContent = new GUIContent("Modifiers Setup");
			wnd.minSize = new Vector2(340, 64);
			wnd.maxSize = new Vector2(340, 128);
			wnd.ShowModal();
		}

		[MenuItem("Tools/InsaneOne Modifiers/Generate constants", priority = 9999)]
		public static void MenuGenerateConstants() => ConstsGenerator.Generate();

		void OnGUI()
		{
			if (UnityModifiersSettings.TryGetEditor(out _))
			{
				GUILayout.Label("Setup is finished!");

				if (GUILayout.Button("Close window"))
					Close();

				return;
			}

			GUILayout.Label("Looks like InsaneOne.Modifiers was not setup before\n(No DefaultUnitModifiersSettings asset found).");

			var prevColor = GUI.color;
			GUI.color = Color.green;

			if (GUILayout.Button("Setup Modifiers"))
				Init();

			GUI.color = prevColor;

			if (GUILayout.Button("Dismiss in current session"))
				Dismiss();
		}

		void Dismiss()
		{
			SessionState.SetBool(DismissedSessionKey, true);
			Close();
		}

		void Init()
		{
			var newData = ScriptableObject.CreateInstance<UnityModifiersSettings>();

			if (!AssetDatabase.IsValidFolder("Assets/Resources"))
				AssetDatabase.CreateFolder("Assets", "Resources");

			if (!AssetDatabase.IsValidFolder("Assets/Resources/InsaneOne"))
				AssetDatabase.CreateFolder("Assets/Resources", "InsaneOne");

			AssetDatabase.Refresh();
			AssetDatabase.CreateAsset(newData, "Assets/Resources/InsaneOne/DefaultModifierSettings.asset");

			Debug.Log("New Modifiers config was <b><color=#55ff33>created</color></b>.");
		}
	}
}
#endif