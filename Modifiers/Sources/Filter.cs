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
		public bool IsExclude { get; private set; }

		float compareTolerance = 0.01f;

		readonly List<GameObject> all = new ();
		readonly List<GameObject> activeResults = new ();
		
		Filter(string param, float value)
		{
			ParamType = param;
			Value = value;

			var results = Modifiable.FindAllWith(param, value, compareTolerance);
			foreach (var go in results)
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

				if (IsMatchesFilter(filter, go) && !filter.all.Contains(go))
					filter.all.Add(go);
				else if (!IsMatchesFilter(filter, go) && filter.all.Contains(go))
					filter.all.Remove(go);
			}
		}

		public static Filter Make(string type, float value, bool isExclude = false)
		{
			foreach (var filter in filters)
				if (filter.ParamType == type && IsMatchesFilter(filter, value))
					return filter;
			
			return new Filter(type, value) {IsExclude = isExclude};
		}

		internal static void RemoveAll(GameObject go)
		{
			foreach (var filter in filters)
				filter.all.Remove(go);
		}
		
		internal static void InjectInAll(GameObject go)
		{
			foreach (var filter in filters)
				if (IsMatchesFilter(filter, go) && !filter.all.Contains(go))
					filter.all.Add(go);
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