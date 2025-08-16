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

#if INSANEONE_MODIFIERS_UNITY_EXTENSION

using System;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	/// <summary> Can be used instead of MonoBehaviour to reduce code size. </summary>
	[RequireComponent(typeof(Modifiable))]
	public abstract class ModifiableBehaviour : MonoBehaviour
	{
		public void SetValue(string type, float value) => gameObject.SetModifierValue(type, value);
		public void AddValue(string type, float value) => gameObject.AddModifierValue(type, value);
		public float GetValue(string type) => gameObject.GetModifierValue(type);
		public float GetRawValue(string type) => gameObject.GetModifierRawValue(type);
		public bool IsValueTrue(string type) => GetValue(type) > 0;
		
		public void AddTag(params string[] tags) => gameObject.AddTag(tags);
		public void AddTagOnce(params string[] tags) => gameObject.AddTagOnce(tags);
		public void RemoveTag(params string[] tags) => gameObject.RemoveTag(tags);
		public bool HasTag(string tag) => gameObject.HasTag(tag);
		public bool HasAnyTag(params string[] tags) => gameObject.HasAnyTags(tags);
		public bool HasAllTag(params string[] tags) => gameObject.HasAllTags(tags);

		public void AddModifier(UnityModifier modifier) => gameObject.AddModifier(modifier);
		public void RemoveModifier(UnityModifier modifier) => gameObject.RemoveModifier(modifier);
		
		public void SubTo(string type, Action<float> action) => gameObject.SubToModifier(type, action);
		public void UnsubFrom(string type, Action<float> action) => gameObject.UnsubFromModifier(type, action);
	}
}

#endif