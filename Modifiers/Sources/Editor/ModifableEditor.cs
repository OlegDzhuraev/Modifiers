using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Dev
{
	[CustomEditor(typeof(Modifable))]
	public class ModifableEditor : Editor
	{
		Editor modifierEditor;
		
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var modifable = target as Modifable;

			if (!Application.isPlaying && modifable.DefaultModifier)
			{
				CreateCachedEditor(modifable.DefaultModifier, null, ref modifierEditor);
				modifierEditor.OnInspectorGUI();
			}

			var values = modifable.GetAllValuesInternal();
			
			foreach (var kvp in values)
			{
				GUILayout.Label($"{kvp.Key} = {kvp.Value}", EditorStyles.boldLabel);
				GUILayout.Space(5);
			}
		}
	}
}