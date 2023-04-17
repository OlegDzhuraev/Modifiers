#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Tools
{
	public class UnityCsvExport : EditorWindow
	{
		[SerializeField] Modifier[] modifiers;
		
		SerializedObject so;

		[MenuItem("Tools/InsaneOne/Modifiers CSV Export")]
		static void Init()
		{
			var window = (UnityCsvExport)GetWindow(typeof(UnityCsvExport));
			window.titleContent = new GUIContent("Modifiers CSV Export");
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
			
			EditorGUILayout.PropertyField(serialProp, true);
			so.ApplyModifiedProperties();
			
			var prevEnabled = GUI.enabled;
			GUI.enabled = modifiers.Length > 0;
			
			if (GUILayout.Button("Export CSV to console"))
				Debug.Log(CsvExport.Export(modifiers));

			GUI.enabled = prevEnabled;
		}
	}
}
#endif