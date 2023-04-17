using System;
using UnityEngine;

namespace InsaneOne.Modifiers.Tools
{
	public static class CsvExport
	{
		const int MaxAmount = 99;
		
		public static string Export(Modifier[] modifiers)
		{
			var table = new string[MaxAmount, modifiers.Length + 1];
			var actualParam = 1;

			table[0, 0] = "M_Title";

			for (var y = 1; y <= modifiers.Length; y++)
			{
				var modifier = modifiers[y - 1];
				var innerValues = modifier.GetAllValuesInternal();
				
				foreach (var param in innerValues)
				{
					var isFound = false;

					for (var x = 0; x < MaxAmount - 1; x++)
					{
						if (table[x, 0] == param.Type.ToString())
						{
							table[x, y] = param.Value.ToString();
							isFound = true;
							break;
						}
					}

					table[0, y] = modifier.name;
					
					if (!isFound)
					{
						table[actualParam, 0] = param.Type.ToString();
						table[actualParam, y] = param.Value.ToString();
						actualParam++;
					}
				}
			}

			var resultString = "";

			for (var y = 0; y < table.GetLength(1); y++)
			{
				for (var x = 0; x < actualParam; x++)
					resultString += table[x, y] + (x == actualParam ? "" :";");

				resultString += "\n";
			}

			return resultString;
		}

		public static void Import(string csv, Modifier[] toModifiers)
		{
			throw new NotImplementedException();
		}
	}
}