using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	public class Modifable : MonoBehaviour
	{
		internal static readonly Dictionary<GameObject, Modifable> all = new Dictionary<GameObject, Modifable>();
		
		public event Action WasChanged;
		
		[SerializeField] Modifier defaultModifier;

		public Modifier DefaultModifier => defaultModifier;
		
		readonly List<Modifier> modifiers = new List<Modifier>();
		readonly Dictionary<ModType, float> values = new Dictionary<ModType, float>();

		void Awake()
		{
			all.Add(gameObject, this);
			
			if (defaultModifier)
				Add(defaultModifier);
		}

		void OnDestroy() => all.Remove(gameObject);

		public void Add(Modifier modifier)
		{
			var modifierParams = modifier.GetAllValues();
			
			foreach (var param in modifierParams)
				AddValue(param.Type, param.Value);

			modifiers.Add(modifier);
			WasChanged?.Invoke();
		}

		public void Remove(Modifier modifier)
		{
			if (!modifiers.Contains(modifier))
				return;
			
			var modifierParams = modifier.GetAllValues();
			
			foreach (var param in modifierParams)
				values[param.Type] -= param.Value;
			
			modifiers.Remove(modifier);
			WasChanged?.Invoke();
		}
		
		/// <summary> Sets value to the specified field. Overrides all applied modifiers (can cause wrong results if you will remove some added modifiers after setting custom value, so, be careful). </summary>
		public void SetValue(ModType type, float value) => values[type] = value;

		public void AddValue(ModType type, float value)
		{
			if (values.ContainsKey(type))
				values[type] += value;
			else
				SetValue(type, value);
		}
		
		public float GetValue(ModType type) => values.TryGetValue(type, out var result) ? result : 0;
		public bool IsTrue(ModType type) => GetValue(type) > 0;

		public Dictionary<ModType, float> GetAllValuesInternal() => values;
	}
}