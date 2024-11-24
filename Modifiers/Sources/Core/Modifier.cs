using System;
using System.Collections.Generic;

namespace InsaneOne.Modifiers
{
	[Serializable]
	public class Modifier
	{
#if UNITY_5_3_OR_NEWER
		[UnityEngine.HideInInspector]
#endif
		public string Name;
		
#if UNITY_5_3_OR_NEWER
		[UnityEngine.Serialization.FormerlySerializedAs("parameters")] [UnityEngine.SerializeField]
#endif
		public List<ModifierParam> Parameters = new ();
		
		public bool IsEmpty() => Parameters.Count == 0;

		public float GetRawValue(string type)
		{
			var result = new ModifierParam { Value = 0 };

			foreach (var modifierParam in Parameters)
				if (modifierParam.Type == type)
				{
					result = modifierParam;

					break;
				}

			return result.Value;
		}

		public bool IsTrue(string type) => GetRawValue(type) > 0;

		/// <summary> Overrides param value with a new one. Use only if you know what you're doing. </summary>
		public void SetParamValue(string type, float value)
		{
			var newParam = new ModifierParam {Type = type, Value = value};

			for (var i = 0; i < Parameters.Count; i++)
			{
				if (Parameters[i].Type == type)
				{
					Parameters[i] = newParam;
					return;
				}
			}

			Parameters.Add(newParam);
		}

		public Modifier Clone() => new () { Parameters = new List<ModifierParam>(Parameters) };
	}
}