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

namespace InsaneOne.Modifiers.Tools
{
	public static class CsvSerialization
	{
		const int MaxCount = 100;
		const char Separator = ';';

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

		public static string Serialize(CsvExportSettings settings)
		{
			var modifiers = settings.Modifiers;
			var table = new string[MaxCount, modifiers.Length + 1];
			var currentParam = 1;

			table[0, 0] = ZeroCellName;

			var requireFilter = settings.ParamsFilter.Length > 0;

			for (var y = 1; y <= modifiers.Length; y++)
			{
				var modifier = modifiers[y - 1];

				foreach (var param in modifier.Parameters)
				{
					if (requireFilter && !settings.IsParamInFilter(param.Type))
						continue;

					var isFound = false;

					for (var x = 0; x < MaxCount - 1; x++)
					{
						if (table[x, 0] == param.Type)
						{
							table[x, y] = param.Value.ToString(formatProvider);
							isFound = true;
							break;
						}
					}

					table[0, y] = modifier.Name;

					if (!isFound)
					{
						table[currentParam, 0] = param.Type;
						table[currentParam, y] = param.Value.ToString(formatProvider);
						currentParam++;
					}
				}
			}

			var resultString = "";

			for (var y = 0; y < table.GetLength(1); y++)
			{
				for (var x = 0; x < currentParam; x++)
				{
					var needSeparator = x < currentParam;
					resultString += table[x, y] + (needSeparator ? Separator : "");
				}

				resultString += "\n";
			}

			return resultString;
		}

		public static List<Modifier> Deserialize(string csv)
		{
			if (string.IsNullOrWhiteSpace(csv))
				throw new FormatException($"Empty CSV passed to deserialization!");

			var rows = csv.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			var headers = rows[0].Split(Separator);
			var modifiers = new List<Modifier>();

			for (var i = 1; i < rows.Length; i++)
			{
				var parts = rows[i].Split(Separator);
				var rowName = parts[0];

				if (string.IsNullOrWhiteSpace(rowName))
					throw new FormatException($"Empty modifier name at row {i}!");

				var values = new List<ModifierParam>();

				for (var j = 1; j < parts.Length; j++)
				{
					if (string.IsNullOrWhiteSpace(parts[j]))
						continue;

					if (float.TryParse(parts[j], NumberStyles.Float, formatProvider, out var parsedValue))
						values.Add(new ModifierParam { Type = headers[j], Value = parsedValue });
					else
						throw new FormatException($"Failed to parse value '{headers[j]}' - '{parts[j]}' of '{rowName}!");
				}

				modifiers.Add(new Modifier { Name = rowName, Parameters = values });
			}

			return modifiers;
		}
	}
}