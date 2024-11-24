using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "NewModifier", menuName = "Modifiers/New modifier")]
	public class UnityModifier : ScriptableObject
	{
		[SerializeField] Modifier modifier;

		public Modifier Modifier => modifier;

#if UNITY_EDITOR
		void OnValidate()
		{
			modifier ??= new Modifier();
			modifier.Name = name;
			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif

		public float GetRawValue(string type) => modifier.GetRawValue(type);
		public bool IsTrue(string type) => modifier.IsTrue(type);
		
		public List<ModifierParam> GetParameters() => modifier.Parameters;
	}
}