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
		[Tooltip("Should load values for modifer from default settings if no value found? Otherwise returns Param with value == 0.")]
		[SerializeField] bool useDefaultIfNoValue = true;

		public Modifier DefaultModifier => defaultModifier;
		
		readonly List<Modifier> modifiers = new List<Modifier>();
		readonly Dictionary<ModType, float> values = new Dictionary<ModType, float>();

		readonly Dictionary<ModType, List<Action<float>>> subscriptions = new Dictionary<ModType, List<Action<float>>>();

		void Awake()
		{
			all.Add(gameObject, this);
			
			if (defaultModifier)
				Add(defaultModifier);
			
			Filter.InjectInAll(gameObject);
		}

		void OnDestroy()
		{
			all.Remove(gameObject);
			
			if (all.Count == 0)
				Filter.filters.Clear();
			else
				Filter.RemoveAll(gameObject);
		}

		void OnValueChange(ModType type, float value)
		{
			if (subscriptions.TryGetValue(type, out var list))
				for (var i = list.Count - 1; i >= 0; i--)
					list[i]?.Invoke(value);
		}
		
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
		public void SetValue(ModType type, float value)
		{
			values[type] = value;
			
			OnValueChange(type, value);
		}

		public void AddValue(ModType type, float value)
		{
			if (values.ContainsKey(type))
				values[type] += value;
			else
				SetValue(type, value);
		}
		
		public float GetValue(ModType type) => values.TryGetValue(type, out var result) ? result : GetDefault(type, useDefaultIfNoValue);
		public bool IsTrue(ModType type) => GetValue(type) > 0;

		public void SubTo(ModType type, Action<float> action)
		{
			if (!subscriptions.TryGetValue(type, out var list))
			{
				list = new List<Action<float>>();
				subscriptions[type] = list;
			}
			
			list.Add(action);
		}
		
		public void UnsubFrom(ModType type, Action<float> action)
		{
			if (subscriptions.TryGetValue(type, out var list))
				list.Remove(action);
		}

		public Dictionary<ModType, float> GetAllValuesInternal() => values;
		
		static float GetDefault(ModType type, bool useDefault)
		{
			if (!useDefault)
				return 0;
			
			var defaultValues = DefaultModifierSettings.Get();

			if (defaultValues)
				return defaultValues.GetParamValue(type);

			return 0;
		}

		internal static List<GameObject> FindAllWith(ModType type, int value)
		{
			var result = new List<GameObject>();

			foreach (var state in all)
				if ((int) state.Value.GetValue(type) == value) // && state.gameObject.activeInHierarchy
					result.Add(state.Key);
			
			return result;
		}

		public static void Init() => all.Clear();
	}
}