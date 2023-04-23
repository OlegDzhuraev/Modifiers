using System;
using System.Collections.Generic;

namespace InsaneOne.Modifiers
{
	[Serializable]
	public sealed class Modifier
	{
		
#if UNITY_5_3_OR_NEWER
		[UnityEngine.HideInInspector]
#endif
		 public string Name;
		
#if UNITY_5_3_OR_NEWER
		[UnityEngine.SerializeField]
#endif
		List<ModifierParam> parameters = new ();
		
		[NonSerialized] readonly Dictionary<string, float> initializedParams = new ();
		[NonSerialized] bool isInitialized;

		/// <summary> Use it when before working with modifier. </summary>
		public void Init()
		{
			if (isInitialized)
				return;
					
			foreach (var param in parameters)
				initializedParams[param.Type] = param.Value;

			isInitialized = true;
		}
		
		public bool IsEmpty() => parameters.Count == 0;
		
		public float GetValue(string type)  
		{
			if (isInitialized && initializedParams.TryGetValue(type, out var initResult))
				return initResult;
			
			foreach (var modifierParam in parameters)
				if (modifierParam.Type == type)
					return modifierParam.Value;
			
			return 0;
		}
		
		public bool IsTrue(string type) => GetValue(type) > 0;

		/// <summary> Returns list of all params. Use only if you know what you doing. </summary>
		public List<ModifierParam> GetAllValues() => parameters;

		/// <summary> Overrides param value with a new one. Use only if you know what you doing. </summary>
		public void SetParamValue(string type, float value)
		{
			var newParam = new ModifierParam {Type = type, Value = value};
			
			for (var i = 0; i < parameters.Count; i++)
			{
				if (parameters[i].Type == type)
				{
					parameters[i] = newParam;
					return;
				}
			}
			
			parameters.Add(newParam);
		}

		public Modifier Clone() => new () { parameters = new List<ModifierParam>(parameters) };
	}
}