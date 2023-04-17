using UnityEngine;

namespace InsaneOne.Modifiers
{
	public sealed class GlobalModifable
	{
		static GameObject instance;

		public static GameObject GetSceneInstance()
		{
			if (instance != null)
				return instance;

			var go = new GameObject("GlobalModifable_Scene");
			var modifable = go.AddComponent<Modifable>();

			instance = go;

			return instance;
		}
	}
}