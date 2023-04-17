using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "NewModifier", menuName = "Modifiers/New modifier")]
	public class Modifier : ScriptableObject
	{
		[SerializeField] protected List<ModifierParam> parameters = new List<ModifierParam>();

		[Tooltip("If set, will load values from Fallback if no value found? If there no Fallback, returns Param with value == 0. Dont make recursive fallback modifiers.")]
		[SerializeField] Modifier fallback;

		public Modifier Fallback => fallback;

		[NonSerialized] readonly Dictionary<int, float> initializedParams = new ();
		[NonSerialized] bool isInitialized;
		
		/// <summary> Use it when before working with modifier. Use only on instances! Keep in mind, that instancing is not recommended, since instance is unique modifier and can be removed by reference of its template after.</summary>
		public void Init()
		{
			if (isInitialized)
				return;
					
			foreach (var param in parameters)
				initializedParams.Add((int)param.Type, param.Value);

			isInitialized = true;
		}

		public float GetValue(ModType type) => GetParamValue(type);
		public bool IsTrue(ModType type) => GetParamValue(type) > 0;

		public List<ModifierParam> GetAllValuesInternal()
		{
			if (!fallback)
				return parameters;
			
			var result = new List<ModifierParam>(parameters);
			var toAdd = new List<ModifierParam>(fallback.parameters);

			for (var i = toAdd.Count - 1; i >= 0; i--)
			{
				var toAddParam = toAdd[i];
				var needToAdd = true;

				foreach (var existingParam in result)
					if (existingParam.Type == toAddParam.Type)
					{
						needToAdd = false;
						break;
					}
				
				if (!needToAdd)
					toAdd.Remove(toAddParam);
			}

			result.AddRange(toAdd);
			
			return result;
		}

		internal float GetParamValue(ModType type)
		{
			if (isInitialized && initializedParams.TryGetValue((int)type, out var initResult))
				return initResult;
			
			foreach (var modifierParam in parameters)
				if (modifierParam.Type == type)
					return modifierParam.Value;
			
			return fallback ? fallback.GetValue(type) : 0;
		}
	}
}