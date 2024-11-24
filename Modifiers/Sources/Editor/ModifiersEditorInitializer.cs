using UnityEditor.Callbacks;

namespace InsaneOne.Modifiers.Dev
{
	public class ModifiersEditorInitializer
	{
		[DidReloadScripts]
		static void Initialize()
		{
			if (!UnityModifiersSettings.TryGetEditor(out _))
				ModifiersInitializeWindow.ShowWindow();
		}
	}
}