using System;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "DefaultModifierSettings", menuName = "Modifiers/Default modifier settings")]
	public sealed class DefaultUnityModifierSettings : UnityModifier
	{
		static DefaultUnityModifierSettings instance;

		[SerializeField] string[] supportedModifiers = Array.Empty<string>();

		public string[] SupportedMofifiers => supportedModifiers;

		public static DefaultUnityModifierSettings Get()
		{
			if (!instance)
			{
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