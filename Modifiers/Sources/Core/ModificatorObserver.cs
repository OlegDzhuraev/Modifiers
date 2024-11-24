using System;
using System.Collections.Generic;

namespace InsaneOne.Modifiers
{
	public class ModificatorObserver
	{
		readonly Dictionary<string, List<Action<float>>> subscriptions = new ();

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

		internal void NotifyValueChange(string type, float value)
		{
			if (subscriptions.TryGetValue(type, out var list))
				for (var i = list.Count - 1; i >= 0; i--)
					list[i]?.Invoke(value);
		}
	}
}