using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	public class Modifable : MonoBehaviour
	{
		internal static readonly Dictionary<GameObject, Modifable> all = new ();
		static readonly List<GameObject> searchCollection = new ();
		
		public event Action WasChanged;
		
		[SerializeField] UnityModifier[] defaultModifiers = Array.Empty<UnityModifier>();
		[Tooltip("Should load values for modifer from default settings if no value found? Otherwise returns Param with value == 0.")]
		[SerializeField] bool useDefaultIfNoValue = true;

		public UnityModifier[] DefaultModifiers => defaultModifiers;
		
		readonly List<Modifier> modifiers = new ();
		readonly Dictionary<string, float> values = new ();

		readonly Dictionary<string, List<Action<float>>> subscriptions = new ();

		bool isInitialized;

		void Awake()
		{
			all.Add(gameObject, this);

			foreach (var mod in defaultModifiers)
			{
				var raw = mod.GetRaw();
				raw.Clone();
				raw.Init();
				
				Add(raw);
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

		void OnValueChange(string type, float value)
		{
			if (subscriptions.TryGetValue(type, out var list))
				for (var i = list.Count - 1; i >= 0; i--)
					list[i]?.Invoke(value);
		}
		
		public void Add(UnityModifier modifier) => Add(modifier.GetRaw());
		public void Remove(UnityModifier modifier) => Remove(modifier.GetRaw());

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
				SetValue(param.Type, values[param.Type] - param.Value);

			modifiers.Remove(modifier);
			WasChanged?.Invoke();
		}
		
		/// <summary> Sets value to the specified field. Overrides all applied modifiers (can cause wrong results if you will remove some added modifiers after setting custom value, so, be careful). </summary>
		public void SetValue(string type, float value)
		{
			values[type] = value;
			
			OnValueChange(type, value);
		}

		public void AddValue(string type, float value)
		{
			if (values.ContainsKey(type))
				SetValue(type, values[type] + value);
			else
				SetValue(type, value);
		}
		
		public float GetValue(string type)
		{
			if (!isInitialized)
				return defaultModifiers.Length > 0 ? defaultModifiers[0].GetValue(type) : GetDefault(type, useDefaultIfNoValue);
			
			return values.TryGetValue(type, out var result) ? result : GetDefault(type, useDefaultIfNoValue);
		}

		public bool IsTrue(string type) => GetValue(type) > 0;

		public void SubTo(string type, Action<float> action)
		{
			if (!subscriptions.TryGetValue(type, out var list))
			{
				list = new List<Action<float>>();
				subscriptions[type] = list;
			}
			
			list.Add(action);
		}
		
		public void UnsubFrom(string type, Action<float> action)
		{
			if (subscriptions.TryGetValue(type, out var list))
				list.Remove(action);
		}

		public Dictionary<string, float> GetAllValuesInternal() => values;
		
		static float GetDefault(string type, bool useDefault)
		{
			if (!useDefault)
				return 0;
			
			var defaultValues = DefaultUnityModifierSettings.Get();

			if (defaultValues)
				return defaultValues.GetValue(type);

			return 0;
		}

		internal static List<GameObject> FindAllWith(string type, float value, float tolerance = 0.01f)
		{
			searchCollection.Clear();

			foreach (var state in all)
				if (Math.Abs(state.Value.GetValue(type) - value) < tolerance) // && state.gameObject.activeInHierarchy
					searchCollection.Add(state.Key);
			
			return searchCollection;
		}

        /// <summary> Call this method on game initialization to initialize Modifables system. </summary>
		public static void Init() => all.Clear();
	}
}