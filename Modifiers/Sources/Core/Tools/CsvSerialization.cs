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
using System.Globalization;
using System.Linq;
using System.Text;

namespace InsaneOne.Modifiers.Tools
{
	// todo insaneone - maybe make universal matrix importer? To support not only Modifiers data
	public class CsvSerialization
	{
		public const char Separator = ';';
		public const char RowSeparator = '\n';
		static readonly string ZeroCellName = "M_Title";
		static readonly CultureInfo formatProvider = new ("de-DE"); // for commas in numbers (1,23)

		/*
		 *	|-------------------------------------------|
		 *	| Name		| Value A	| Value B	| etc.	|
		 *	|-------------------------------------------|
		 *	| Asset A	|	1		|	2		|	-	|
		 *	|-------------------------------------------|
		 *	| Asset B	|	0		|	3,5		|	-	|
		 *	|-------------------------------------------|
		 *	| etc.		|	-		|	-		|	-	|
		 *	|-------------------------------------------|
		 */

		public string Serialize(CsvExportSettings settings)
		{
			var modifiers = settings.Modifiers;
			var requireFilter = settings.ParamsFilter.Length > 0;
			var supportedParams = settings.SupportedParams.ToList();
			var sortedParamNames = new List<string>();

			for (var y = 1; y <= modifiers.Length; y++)
			{
				var modifier = modifiers[y - 1];

				foreach (var param in modifier.Parameters)
				{
					if (requireFilter && !settings.IsParamInFilter(param.Type))
						continue;
					
					if (!sortedParamNames.Contains(param.Type))
						sortedParamNames.Add(param.Type);
				}
			}
			
			var table = new string[sortedParamNames.Count + 1, modifiers.Length + 1];
			table[0, 0] = ZeroCellName;

			sortedParamNames = sortedParamNames.OrderBy(paramType =>
				{
					var paramData = supportedParams.First(pd => pd.Name.Equals(paramType));
					return supportedParams.IndexOf(paramData);
				})
				.ToList(); // to always place parameters in same order

			for (var x = 1; x < sortedParamNames.Count; x++)
			{
				var paramName = sortedParamNames[x - 1];
				table[x, 0] = paramName;
			}

			for (var y = 1; y <= modifiers.Length; y++)
			{
				var modifier = modifiers[y - 1];
				
				foreach (var param in modifier.Parameters)
				{
					var xIndex = sortedParamNames.IndexOf(param.Type);
					
					if (xIndex == -1)
						continue;

					table[0, y] = modifier.Name;

					var offsetedIndex = xIndex + 1;
					table[offsetedIndex, y] = param.Value.ToString(formatProvider);
				}
			}

			var sb = new StringBuilder();
			var columnsCount = table.GetLength(0);
			
			for (var y = 0; y < table.GetLength(1); y++)
			{
				for (var x = 0; x < columnsCount; x++)
				{
					var needSeparator = x < columnsCount - 2;
					sb.Append(table[x, y]);

					if (needSeparator)
						sb.Append(Separator);
				}

				sb.AppendLine();
			}

			return sb.ToString();
		}

		public List<Modifier> Deserialize(string csv)
		{
			if (string.IsNullOrWhiteSpace(csv))
				throw new FormatException($"Empty CSV passed to deserialization!");

			var rows = csv.Split(new[] { RowSeparator }, StringSplitOptions.RemoveEmptyEntries);
			var headers = rows[0].Split(Separator);
			var modifiers = new List<Modifier>();

			for (var y = 1; y < rows.Length; y++)
			{
				var parts = rows[y].Split(Separator);
				var rowName = parts[0];

				if (string.IsNullOrWhiteSpace(rowName))
					throw new FormatException($"Empty modifier name at row {y}!");

				var values = new List<ModifierParam>();

				for (var x = 1; x < parts.Length; x++)
				{
					if (string.IsNullOrWhiteSpace(parts[x]))
						continue;

					if (float.TryParse(parts[x], NumberStyles.Float, formatProvider, out var parsedValue))
						values.Add(new ModifierParam { Type = headers[x], Value = parsedValue });
					else
						throw new FormatException($"Failed to parse value '{headers[x]}' - '{parts[x]}' of '{rowName}!");
				}

				modifiers.Add(new Modifier { Name = rowName, Parameters = values });
			}

			return modifiers;
		}
	}
}