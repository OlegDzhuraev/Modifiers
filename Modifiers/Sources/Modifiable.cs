/*
 * Copyright 2025 Oleg Dzhuraev <godlikeaurora@gmail.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#if UNITY_5_3_OR_NEWER
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[DisallowMultipleComponent]
	public class Modifiable : MonoBehaviour
	{
		internal static readonly Dictionary<GameObject, Modifiable> all = new ();
		static readonly List<GameObject> searchCollection = new ();
		
		public event Action WasChanged;
		
		[SerializeField] UnityModifier[] defaultModifiers = Array.Empty<UnityModifier>();

		public UnityModifier[] DefaultModifiers => defaultModifiers;
		
		readonly List<Modifier> modifiers = new ();

		/// <summary> All calculations processed in this modifer. </summary>
		RuntimeModifier baseModifier;

		void Awake()
		{
			all.Add(gameObject, this);

			baseModifier = new RuntimeModifier(UnityModifiersSettings.Get());

			foreach (var mod in defaultModifiers)
				Add(mod.Modifier);

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

		public void Add(UnityModifier modifier) => Add(modifier.Modifier);
		public void Remove(UnityModifier modifier) => Remove(modifier.Modifier);

		public void Add(Modifier modifier)
		{
			baseModifier.Add(modifier);
			modifiers.Add(modifier);

			NotifyFiltersChanged(modifier);
			WasChanged?.Invoke();
		}

		public void Remove(Modifier modifier)
		{
			if (!modifiers.Contains(modifier))
				return;

			baseModifier.Remove(modifier);

			modifiers.Remove(modifier);

			NotifyFiltersChanged(modifier);
			WasChanged?.Invoke();
		}

		/// <summary> Sets value to the specified field. Overrides all applied modifiers (can cause wrong results if you will remove some added modifiers after setting custom value, so, be careful). </summary>
		public void SetValue(string type, float value)
		{
			baseModifier.SetValue(type, value);

			Filter.UpdateAll(gameObject, new ModifierParam { Type = type });
			WasChanged?.Invoke();
		}

		/// <summary> Manually adds value. Use only if you know what are you doing! </summary>
		public void AddValue(string type, float value)
		{
			var currentValue = GetValue(type);
			SetValue(type, currentValue + value);
		}

		/// <summary> Re-checks every registered Filter for the parameter types this modifier touches, so filters
		/// stay in sync when a modifier is added/removed after they were created (Filter.Value itself is not read
		/// here - Filter.UpdateAll re-fetches the current value from this Modifiable). </summary>
		void NotifyFiltersChanged(Modifier modifier)
		{
			foreach (var param in modifier.Parameters)
				Filter.UpdateAll(gameObject, new ModifierParam { Type = param.Type });
		}

		public float GetValue(string type)
		{
			if (!Application.isPlaying)
				return defaultModifiers.Length > 0 ? defaultModifiers[0].GetRawValue(type) : 0;

			var isParamSet = baseModifier.TryGetValue(type, out var result);
			return isParamSet ? result : 0;
		}

		public bool IsTrue(string type) => GetValue(type) > 0;

		public void SubTo(string type, ModifierChangedCallback action) => baseModifier.Observer.SubTo(type, action);
		public void UnsubFrom(string type, ModifierChangedCallback action) => baseModifier.Observer.UnsubFrom(type, action);

#if INSANEONE_MODIFIERS_UNITY_EXTENSION
		public static void TransferModifiers(GameObject to, GameObject from, List<string> modifiersTypes, bool ignoreZeroes = true)
		{
			foreach (var modType in modifiersTypes)
			{
				var value = from.GetModifierValue(modType);

				if (ignoreZeroes && Mathf.Approximately(value, 0))
					continue;

				to.SetModifierValue(modType, value);
			}
		}
#endif

#if UNITY_EDITOR
		/// <summary> Editor only feature. </summary>
		public void AddDefault(UnityModifier modifier)
		{
			Array.Resize(ref defaultModifiers, defaultModifiers.Length + 1);
			defaultModifiers[^1] = modifier;
			UnityEditor.EditorUtility.SetDirty(this);
		}

		/// <summary> Editor only feature. Do not use it to modify inner state. </summary>
		public Dictionary<string, ModifierParam> GetAllValuesInternal() => baseModifier?.GetValuesInternal();

		/// <summary> Do not use it to modify inner state. Editor only feature. </summary>
		public List<Modifier> GetAllModifiersInternal() => modifiers;
#endif

		internal static List<GameObject> FindAllWith(string type, float value, float tolerance = 0.01f)
		{
			searchCollection.Clear();

			foreach (var (go, modifiable) in all)
				if (Math.Abs(modifiable.GetValue(type) - value) < tolerance) // && state.gameObject.activeInHierarchy
					searchCollection.Add(go);
			
			return searchCollection;
		}

		/// <summary> Call this method on game initialization to initialize Modifiables system. </summary>
		public static void Init() => all.Clear();

		public float GetRawValue(string type) => baseModifier.GetRawValue(type);

		public void ClearDefault()
		{
			foreach (var def in defaultModifiers)
				Remove(def);

			defaultModifiers = Array.Empty<UnityModifier>();
		}
	}
}
#endif