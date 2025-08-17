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

using System.Collections.Generic;

namespace InsaneOne.Modifiers
{
	public delegate void ModifierChangedCallback(float newValue);

	public class ModificatorObserver
	{
		readonly Dictionary<string, List<ModifierChangedCallback>> subscriptions = new ();

		public void SubTo(string type, ModifierChangedCallback action)
		{
			if (!subscriptions.TryGetValue(type, out var list))
			{
				list = new List<ModifierChangedCallback>();
				subscriptions[type] = list;
			}

			list.Add(action);
		}

		public void UnsubFrom(string type, ModifierChangedCallback action)
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