#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace InsaneOne.Modifiers.Tools
{
	public class UnityCsvExport : EditorWindow
	{
		[SerializeField] UnityModifier[] modifiers;
		
		SerializedObject so;

		[MenuItem("Tools/InsaneOne Modifiers/Export CSV...")]
		public static void Init()
		{
			var window = (UnityCsvExport)GetWindow(typeof(UnityCsvExport));
			window.titleContent = new GUIContent("Modifiers Csv Export");
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
			GUI.enabled = modifiers is {Length: > 0};

			if (GUILayout.Button("Export CSV to console"))
				Debug.Log(MakeExportString(modifiers));
			
			if (GUILayout.Button("DEBBBB GEN "))
				ConstsGenerator.Generate();
			
			if (GUILayout.Button("Export CSV to file in Assets"))
			{
				var path = Path.Combine(Application.dataPath, "ModifiersExport_rid_" + Random.Range(0, 9999) + ".csv");
				File.WriteAllText(path, MakeExportString(modifiers));
			}
			
			GUI.enabled = prevEnabled;
		}

		string MakeExportString(UnityModifier[] mods)
		{
			var rawModifiers = new Modifier[modifiers.Length];
			for (var i = 0; i < rawModifiers.Length; i++)
				rawModifiers[i] = modifiers[i].GetRaw();
				
			return CsvExport.Export(rawModifiers);
		}
	}
}
#endif