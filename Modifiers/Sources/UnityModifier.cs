using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "NewModifier", menuName = "Modifiers/New modifier")]
	public class UnityModifier : ScriptableObject
	{
		[SerializeField] Modifier modifier;

		public Modifier GetRaw() => modifier;
		
		/// <summary> Use it before working with modifier instance. Use only on instances! Keep in mind, that instancing is not recommended, since instance is unique modifier and can be removed by reference of its template after.</summary>
		protected void Init()
		{
			modifier = modifier.Clone();
			modifier.Init();
		}

		void OnValidate()
		{
			modifier ??= new Modifier();
			
			modifier.Name = name;

#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
#endif
		}

		public float GetValue(string type) => modifier.GetValue(type);
		public bool IsTrue(string type) =>modifier.GetValue(type) > 0;
		
		public List<ModifierParam> GetAllValuesInternal() => modifier.GetAllValues();
		
		internal void SetParamValue(string type, float value) => modifier.SetParamValue(type, value);
	}
}