using System.IO;

namespace InsaneOne.Modifiers
{
	public static class ConstsGenerator
	{
		static readonly string tpl = @"namespace #NAMESPACE
{
	public static class #CLASSNAME
	{
#CONTENT
	}
}";

		static readonly string namespaceName = "InsaneOne.Modifiers";
		static readonly string className = "ModType";
		
#if UNITY_EDITOR
		static string path = UnityEngine.Application.dataPath;
#else
		static string path = System.AppDomain.CurrentDomain.BaseDirectory;
#endif
		
		static readonly string constPath = Path.Combine("InsaneOne", "Generated");

		public static void SetCustomPath(string newPath) => path = newPath;

		public static void Generate()
		{
			var result = tpl;
			result = result.Replace("#NAMESPACE", namespaceName);
			result = result.Replace("#CLASSNAME", className);

			var content = "";
			var mods = DefaultUnityModifierSettings.Get().SupportedMofifiers;
			
			foreach (var mod in mods)
				content += $"		public const string {mod} = \"{mod}\";\n";
			
			result = result.Replace("#CONTENT", content);

			MakeDir(Path.Combine(path, "InsaneOne"));
			MakeDir(Path.Combine(path, constPath));

			File.WriteAllText(Path.Combine(path, constPath, className + ".cs"), result);
			
#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh();
			UnityEngine.Debug.Log("[Modifiers] Consts class was generated successfully.");
#endif

		}

		static void MakeDir(string dirPath)
		{
			if (!Directory.Exists(dirPath))
				Directory.CreateDirectory(dirPath);
		}
	}
}