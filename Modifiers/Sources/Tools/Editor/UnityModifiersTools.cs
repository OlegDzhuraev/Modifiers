#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Tools
{
	public class UnityModifiersTools : EditorWindow
	{
		[MenuItem("Tools/InsaneOne Modifiers/Common...")]
		public static void Init()
		{
			var window = (UnityModifiersTools)GetWindow(typeof(UnityModifiersTools));
			window.titleContent = new GUIContent("Common tools");
			window.Show();
		}

		void OnGUI()
		{
			if (GUILayout.Button("CSV Export..."))
				UnityCsvWindow.Init();
			
			if (GUILayout.Button("Generate constants"))
				ConstsGenerator.Generate();
		}
	}
}

#endif