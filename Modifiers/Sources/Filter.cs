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
	public class Filter
	{
		internal static readonly List<Filter> filters = new ();
		
		public string ParamType { get; }
		public float Value { get; }
		public bool IsExclude { get; }

		float compareTolerance = 0.01f;

		// HashSet, not List: UpdateAll/InjectInAll/RemoveAll do Contains/Add/Remove on this for every value
		// change on every tracked Modifiable, so this needs O(1) membership ops rather than a linear scan.
		readonly HashSet<GameObject> all = new ();
		readonly List<GameObject> activeResults = new ();

		Filter(string param, float value, bool isExclude)
		{
			ParamType = param;
			Value = value;
			IsExclude = isExclude; // must be set before populating all, IsMatchesFilter below depends on it

			foreach (var (go, modifiable) in Modifiable.all)
				if (IsMatchesFilter(this, modifiable.GetValue(param)))
					all.Add(go);

			filters.Add(this);
		}

		public List<GameObject> GetResults()
		{
			activeResults.Clear();

			foreach (var go in all)
				if (go.activeInHierarchy)
					activeResults.Add(go);

			return activeResults;
		}

		internal static void UpdateAll(GameObject go, ModifierParam param)
		{
			foreach (var filter in filters)
			{
				if (param.Type != filter.ParamType)
					continue;

				if (IsMatchesFilter(filter, go))
					filter.all.Add(go); // no-op (returns false) if already present
				else
					filter.all.Remove(go); // no-op (returns false) if not present
			}
		}

		public static Filter Make(string type, float value, bool isExclude = false)
		{
			// Reuse an existing filter only if it was defined identically - checking whether `value` currently
			// satisfies the candidate filter's own predicate would ignore a mismatched IsExclude and could hand
			// back a filter with the opposite semantics from the ones requested here.
			foreach (var filter in filters)
				if (filter.ParamType == type && filter.IsExclude == isExclude && Math.Abs(filter.Value - value) < filter.compareTolerance)
					return filter;

			return new Filter(type, value, isExclude);
		}

		internal static void RemoveAll(GameObject go)
		{
			foreach (var filter in filters)
				filter.all.Remove(go);
		}
		
		internal static void InjectInAll(GameObject go)
		{
			foreach (var filter in filters)
				if (IsMatchesFilter(filter, go))
					filter.all.Add(go); // no-op (returns false) if already present
		}

		static bool IsMatchesFilter(Filter filter, GameObject go)
		{
			return Modifiable.all.TryGetValue(go, out var modifiable) && IsMatchesFilter(filter, modifiable.GetValue(filter.ParamType));
		}
		
		static bool IsMatchesFilter(Filter filter, float value)
		{
			var isMatches = Math.Abs(filter.Value - value) < filter.compareTolerance;
			return isMatches && !filter.IsExclude || !isMatches && filter.IsExclude;
		}
	}
}

#endif