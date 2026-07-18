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

#if UNITY_5_3_OR_NEWER && INSANEONE_MODIFIERS_UNITY_EXTENSION
using System;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	/// <summary> Use this structure instead of default float/int, if you want to get specific modifier value from object in runtime, instead of using only value, which set in config (pre-built).
	/// <para>Experimental feature</para></summary>
	[Serializable]
	public struct ModifierField
	{
		[Modifier] public string Modifier;
		[Tooltip("This value will be returned in case field " + nameof(Modifier) + " is not set.")]
		public float DefaultValue;

		/// <summary> Returns value using target modifiers at first, if field <see cref="Modifier"/> set. Otherwise, will return value <see cref="DefaultValue"/>.</summary>
		public float GetValue(GameObject target)
		{
			if (string.IsNullOrEmpty(Modifier) || !target)
				return DefaultValue;

			return target.GetModifierValue(Modifier);
		}

		/// <summary> Returns value using target modifiers at first, if field <see cref="Modifier"/>. Otherwise, will return value <see cref="DefaultValue"/>. Cut to int!</summary>
		public int GetIntValue(GameObject target) => (int) GetValue(target);
	}
}

#endif