using System;
using System.Globalization;

namespace InsaneOne.Modifiers.Tools
{
	public static class CsvExport
	{
		public static string Export(Modifier[] modifiers)
		{
			var table = new string[100, modifiers.Length + 1];
			var actualParam = 1;

			table[0, 0] = "M_Title";

			for (var y = 1; y <= modifiers.Length; y++)
			{
				var modifier = modifiers[y - 1];
				
				foreach (var param in modifier.Parameters)
				{
					var isFound = false;

					for (var x = 0; x < 99; x++)
					{
						if (table[x, 0] == param.Type)
						{
							table[x, y] = param.Value.ToString(CultureInfo.InvariantCulture);
							isFound = true;
							break;
						}
					}

					table[0, y] = modifier.Name;
					
					if (!isFound)
					{
						table[actualParam, 0] = param.Type;
						table[actualParam, y] = param.Value.ToString(CultureInfo.InvariantCulture);
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