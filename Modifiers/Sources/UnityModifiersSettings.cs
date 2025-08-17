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
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace InsaneOne.Modifiers
{
	/// <summary> This class stores all supported modifiers settings. Also, you can set up there default modifier parameters, which can be applied to empty Modifiable object.
	/// For example, you want to some MaxHealth value be 100 by default for any object - you set it there and do not need to manually add this Modifier to every game object,
	/// which has MaxHealth parameter.</summary>
	[CreateAssetMenu(fileName = "DefaultModifierSettings", menuName = "Modifiers/Modifier Settings")]
	public sealed class UnityModifiersSettings : ScriptableObject, IModifiersSettings
	{
		const string DefaultName = "ModifierSettings";

		/// <summary> Used as cache for fast access.</summary>
		static UnityModifiersSettings instance;

		[SerializeField] ModifierParamData[] supportedParams = Array.Empty<ModifierParamData>();

		[FormerlySerializedAs("colorGroups")]
		[Tooltip("Editor enhancement: you can mark your modifiers with color to group them visually.")]
		[SerializeField] List<ParamGroup> groups = new ();

		public ModifierParamData[] SupportedParams => supportedParams;
		public List<ParamGroup> ParamGroups => groups;

		/// <summary> Call it on game initialize to manually set up your own default settings, otherwise it will try to load it from resources. </summary>
		public void SetAsActive() => instance = this;

		public ModifierParamData GetModifierParamData(string paramName)
		{
			foreach (var data in supportedParams)
				if (data.Name == paramName)
					return data;

			return null;
		}

		public Color GetEditorColor(string paramName)
		{
			var data = GetModifierParamData(paramName);
			return data == null ? Color.white : GetEditorGroupColor(data.Group);
		}

		public Color GetEditorGroupColor(string groupName)
		{
			foreach (var group in groups.Where(group => group.Name == groupName))
				return group.Color;

			return Color.white;
		}

		/// <summary> Returns global default settings asset. You need to set up it manually, or it will try to load it from resources (if exist). </summary>
		public static UnityModifiersSettings Get()
		{
			if (!instance)
			{
				Debug.LogWarning($"No Modifier Settings was setup by using {nameof(SetAsActive)} method! Trying to find it in Resources...");

				instance = Resources.Load<UnityModifiersSettings>(DefaultName);
			
				if (!instance)
					Debug.LogWarning("No Modifier Settings asset exist!");
			}

			return instance;
		}

#if UNITY_EDITOR
		public static bool TryGetEditor(out UnityModifiersSettings result)
		{
			if (instance)
			{
				result = instance;
				return true;
			}

			var assets = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(UnityModifiersSettings)}");

			if (assets.Length > 0)
			{
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]);
				result = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityModifiersSettings>(path);
				instance = result;
				return true;
			}

			result = default;
			return false;
		}
#endif
	}

	[Serializable]
	public class ParamGroup
	{
		public string Name;
		public Color Color = Color.white;
	}
}

#endif