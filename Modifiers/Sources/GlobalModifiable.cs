using UnityEngine;

namespace InsaneOne.Modifiers
{
	public sealed class GlobalModifiable
	{
		static GameObject instance;

		public static GameObject GetSceneInstance()
		{
			if (instance != null)
				return instance;

			var go = new GameObject("GlobalModifiable_Scene");
			go.AddComponent<Modifiable>();

			instance = go;

			return instance;
		}
	}
}