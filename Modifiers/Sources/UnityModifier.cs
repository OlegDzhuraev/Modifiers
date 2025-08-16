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
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "NewModifier", menuName = "Modifiers/New modifier")]
	public class UnityModifier : ScriptableObject
	{
		[SerializeField] Modifier modifier;

		public Modifier Modifier => modifier;

#if UNITY_EDITOR
		void OnValidate()
		{
			modifier ??= new Modifier();
			modifier.Name = name;
			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif

		public float GetRawValue(string type) => modifier.GetRawValue(type);
		public bool IsTrue(string type) => modifier.IsTrue(type);
		
		public List<ModifierParam> GetParameters() => modifier.Parameters;

#if UNITY_EDITOR
		/// <summary> Used for import purposes. Be careful with this method! Editor-only now.</summary>
		/// <param name="modifier"></param>
		public void EditorSetModifier(Modifier modifier)
		{
			this.modifier = modifier;
			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif
	}
}