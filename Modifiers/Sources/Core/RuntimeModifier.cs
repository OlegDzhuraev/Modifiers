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

namespace InsaneOne.Modifiers
{
	public class RuntimeModifier
	{
		public readonly ModificatorObserver Observer;
		public readonly  IModifiersSettings Settings;

		readonly Dictionary<string, ModifierParam> initializedParams = new ();

		public RuntimeModifier(IModifiersSettings modifiersSettings)
		{
			Settings = modifiersSettings;
			Observer = new ModificatorObserver();
		}

		public RuntimeModifier(Modifier startModifier, IModifiersSettings modifiersSettings)
		{
			Add(startModifier);
			Settings = modifiersSettings;
			Observer = new ModificatorObserver();
		}

		/// <summary> Returns processed value of specified param. If there are no processors, value will be same to raw value. </summary>
		public bool TryGetValue(string type, out float result)
		{
			if (initializedParams.TryGetValue(type, out var param))
			{
				result = param.GetProcessedValue(Settings);
				return true;
			}

			result = 0;
			return false;
		}

		/// <summary> Returns processed value of specified param. If there are no processors, value will be same to raw value. </summary>
		public float GetValue(string type) => initializedParams.TryGetValue(type, out var result) ?  result.GetProcessedValue(Settings) : 0;
		public float GetRawValue(string type) => initializedParams.TryGetValue(type, out var result) ? result.Value : 0;

		/// <summary> Checks if value not equal to zero. Note: it uses unprocessed value to run this check. </summary>
		public bool IsTrue(string type) => GetRawValue(type) > 0;

		public void Add(Modifier modifier)
		{
			var modifierParams = modifier.Parameters;

			foreach (var param in modifierParams)
				AddValue(param.Type, param.Value);
		}

		public void Remove(Modifier modifier)
		{
			var modifierParams = modifier.Parameters;

			foreach (var param in modifierParams)
				SetValue(param.Type, initializedParams[param.Type].Value - param.Value);
		}

		/// <summary> Sets value to the specified field. Overrides all applied modifiers (can cause wrong results if you will remove some added modifiers after setting custom value, so, be careful). </summary>
		public void SetValue(string type, float value)
		{
			if (initializedParams.TryGetValue(type, out var param))
				param.Value = value;
			else
				param = new ModifierParam { Type = type, Value = value };

			initializedParams[type] = param;

			Observer?.NotifyValueChange(type, GetValue(type));
		}

		public void AddValue(string type, float value)
		{
			if (initializedParams.TryGetValue(type, out var param))
				SetValue(type, param.Value + value);
			else
				SetValue(type, value);
		}

#if UNITY_EDITOR
		/// <summary> Editor only feature. Returns dict of all params. Do not modify this manually. </summary>
		public Dictionary<string, ModifierParam> GetValuesInternal() => initializedParams;
#endif
	}
}