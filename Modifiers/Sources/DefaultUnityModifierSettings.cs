using System;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "DefaultModifierSettings", menuName = "Modifiers/Default modifier settings")]
	public sealed class DefaultUnityModifierSettings : UnityModifier
	{
		static DefaultUnityModifierSettings instance;

		[SerializeField] string[] supportedModifiers = Array.Empty<string>();

		public string[] SupportedModifiers => supportedModifiers;

		/// <summary> Call it on game initialize to manually set up your own default settings, otherwise it will try to load it from resources. </summary>
		public static void Setup(DefaultUnityModifierSettings defaultSettings) => instance = defaultSettings;

		/// <summary> Returns global default settings asset. You need to setup it manually, or it will try to load it from resources (if exist). </summary>
		public static DefaultUnityModifierSettings Get()
		{
			if (!instance)
			{
				Debug.LogWarning($"No Default Unity Modifier Settings was setup by using {nameof(Setup)} method! Trying to find it in Resources...");

				instance = Resources.Load<DefaultUnityModifierSettings>("DefaultModifierSettings");
				
				if (instance)
					instance.Init();
				else 
					Debug.LogWarning("No Default Unity Modifier Settings asset exist!");
			}

			return instance;
		}
	}
}