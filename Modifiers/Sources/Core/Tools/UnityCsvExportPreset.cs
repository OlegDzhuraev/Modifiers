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
using UnityEngine;

namespace InsaneOne.Modifiers.Tools
{
	[CreateAssetMenu(fileName = "CSVExportPreset", menuName = "Modifiers/New CSV export preset")]
	public class CsvExportPreset : ScriptableObject
	{
		[Tooltip("Collection of modifiers to export.")]
		[SerializeField] UnityModifier[] modifiers = new UnityModifier[0];

		[Tooltip("If empty, no filter will be applied")]
		[SerializeField, Modifier] string[] paramsFilter;

		public bool IsEmpty() => modifiers == null || modifiers.Length == 0;

		public CsvExportSettings GetExportSettings()
		{
			var settings = new CsvExportSettings
			{
				Modifiers = new Modifier[modifiers.Length],
				ParamsFilter = paramsFilter,
			};

			for (var i = 0; i < modifiers.Length; i++)
			{
				var unityModifier = modifiers[i];
				settings.Modifiers[i] = unityModifier.Modifier;
			}

			return settings;
		}
	}
}

#endif