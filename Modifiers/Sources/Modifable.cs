using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	public class Modifable : MonoBehaviour
	{
		internal static readonly Dictionary<GameObject, Modifable> all = new Dictionary<GameObject, Modifable>();
		static readonly List<GameObject> searchLsit = new List<GameObject>();
		
		public event Action WasChanged;
		
		[SerializeField] Modifier defaultModifier;
		[Tooltip("Should load values for modifer from default settings if no value found? Otherwise returns Param with value == 0.")]
		[SerializeField] bool useDefaultIfNoValue = true;

		public Modifier DefaultModifier => defaultModifier;
		
		readonly List<Modifier> modifiers = new List<Modifier>();
		readonly Dictionary<int, float> values = new ();

		readonly Dictionary<int, List<Action<float>>> subscriptions = new ();

		bool isInitialized;

		void Awake()
		{
			all.Add(gameObject, this);

			if (defaultModifier)
			{
				var instanced = Instantiate(defaultModifier);
				instanced.Init();
				Add(instanced);
			}

			Filter.InjectInAll(gameObject);

			isInitialized = true;
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
			if (subscriptions.TryGetValue((int)type, out var list))
				for (var i = list.Count - 1; i >= 0; i--)
					list[i]?.Invoke(value);
		}
		
		public void Add(Modifier modifier)
		{
			var modifierParams = modifier.GetAllValuesInternal();
			
			foreach (var param in modifierParams)
				AddValue(param.Type, param.Value);

			modifiers.Add(modifier);
			WasChanged?.Invoke();
		}

		public void Remove(Modifier modifier)
		{
			if (!modifiers.Contains(modifier))
				return;
			
			var modifierParams = modifier.GetAllValuesInternal();

			foreach (var param in modifierParams)
				SetValue(param.Type, values[(int)param.Type] - param.Value);

			modifiers.Remove(modifier);
			WasChanged?.Invoke();
		}
		
		/// <summary> Sets value to the specified field. Overrides all applied modifiers (can cause wrong results if you will remove some added modifiers after setting custom value, so, be careful). </summary>
		public void SetValue(ModType type, float value)
		{
			values[(int)type] = value;
			
			OnValueChange(type, value);
		}

		public void AddValue(ModType type, float value)
		{
			if (values.ContainsKey((int)type))
				SetValue(type, values[(int)type] + value);
			else
				SetValue(type, value);
		}
		
		public float GetValue(ModType type)
		{
			if (!isInitialized)
				return defaultModifier ? defaultModifier.GetValue(type) : GetDefault(type, useDefaultIfNoValue);
			
			return values.TryGetValue((int)type, out var result) ? result : GetDefault(type, useDefaultIfNoValue);
		}

		public bool IsTrue(ModType type) => GetValue(type) > 0;

		public void SubTo(ModType type, Action<float> action)
		{
			if (!subscriptions.TryGetValue((int)type, out var list))
			{
				list = new List<Action<float>>();
				subscriptions[(int)type] = list;
			}
			
			list.Add(action);
		}
		
		public void UnsubFrom(ModType type, Action<float> action)
		{
			if (subscriptions.TryGetValue((int)type, out var list))
				list.Remove(action);
		}

		public Dictionary<int, float> GetAllValuesInternal() => values;
		
		static float GetDefault(ModType type, bool useDefault)
		{
			if (!useDefault)
				return 0;
			
			var defaultValues = DefaultModifierSettings.Get();

			if (defaultValues)
				return defaultValues.GetParamValue(type);

			return 0;
		}

		internal static List<GameObject> FindAllWith(ModType type, float value, float tolerance = 0.01f)
		{
			searchLsit.Clear();

			foreach (var state in all)
				if (Math.Abs(state.Value.GetValue(type) - value) < tolerance) // && state.gameObject.activeInHierarchy
					searchLsit.Add(state.Key);
			
			return searchLsit;
		}

		public static void Init() => all.Clear();
	}
}