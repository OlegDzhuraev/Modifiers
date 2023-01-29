using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	public class Filter
	{
		internal static readonly List<Filter> filters = new ();
		
		public ModType ParamType { get; }
		public int Value { get; }

		readonly List<GameObject> all = new ();
		readonly List<GameObject> activeResults = new ();
		
		Filter(ModType param, int value)
		{
			ParamType = param;
			Value = value;

			var results = Modifable.FindAllWith(param, value);
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
			var newValue = (int) param.Value;
			
			foreach (var filter in filters)
			{
				if (param.Type != filter.ParamType)
					continue;

				if (newValue == filter.Value && !filter.all.Contains(go))
					filter.all.Add(go);
				else if (newValue != filter.Value && filter.all.Contains(go))
					filter.all.Remove(go);
			}
		}

		public static Filter Make(ModType type, int value)
		{
			foreach (var filter in filters)
				if (filter.ParamType == type && filter.Value == value)
					return filter;
			
			return new Filter(type, value);
		}

		internal static void RemoveAll(GameObject go)
		{
			foreach (var filter in filters)
				filter.all.Remove(go);
		}
		
		internal static void InjectInAll(GameObject go)
		{
			foreach (var filter in filters)
			{
				var paramValue = (int)go.GetModifierValue(filter.ParamType);
			
				if (paramValue == filter.Value && !filter.all.Contains(go))
					filter.all.Add(go);
			}
		}
	}
}