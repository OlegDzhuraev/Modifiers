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
using System.Text;
using UnityEngine;

namespace InsaneOne.Modifiers.Tools
{
	public class CsvParamGroupsSerializer
	{
		public string Serialize(IEnumerable<ParamGroup> paramGroups)
		{
			var sb = new StringBuilder();
			
			foreach (var paramGroup in paramGroups)
			{
				var colorStr = ColorUtility.ToHtmlStringRGB(paramGroup.Color);
				sb.Append(paramGroup.Name);
				sb.Append(CsvSerializer.Separator);
				sb.Append(colorStr);
				sb.Append(CsvSerializer.RowSeparator);
			}

			return sb.ToString();
		}

		public ParamGroup[] Deserialize(string csvString)
		{
			var rows = csvString.Split(new[] { CsvSerializer.RowSeparator }, StringSplitOptions.RemoveEmptyEntries);
			var result = new ParamGroup[rows.Length]; // todo insaneone.modifiers: -1?

			for (var y = 0; y < rows.Length; y++)
			{
				var parts = rows[y].Split(CsvSerializer.Separator);

				if (!ColorUtility.TryParseHtmlString(parts[1], out var color))
					throw new Exception("Wrong group color in input csvString! It should be in HEX format (like HTML).");
				
				result[y] = new ParamGroup { Name = parts[0], Color = color };
			}

			return result;
		}
	}
}

#endif