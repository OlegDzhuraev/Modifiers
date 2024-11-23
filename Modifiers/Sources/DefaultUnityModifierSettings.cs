using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "DefaultModifierSettings", menuName = "Modifiers/Default modifier settings")]
	public sealed class DefaultUnityModifierSettings : UnityModifier
	{
		/// <summary> Used as cache for fast access.</summary>
		static DefaultUnityModifierSettings instance;

		[SerializeField] string[] supportedModifiers = Array.Empty<string>();

		[Tooltip("Editor enhancement: you can mark your modifiers with color to group them visually.")]
		[SerializeField] List<ColorGroup> colorGroups = new List<ColorGroup>();

		public string[] SupportedModifiers => supportedModifiers;

		/// <summary> Call it on game initialize to manually set up your own default settings, otherwise it will try to load it from resources. </summary>
		public static void Setup(DefaultUnityModifierSettings defaultSettings) => instance = defaultSettings;

		public Color GetEditorColor(string modifierName)
		{
			foreach (var colorGroup in colorGroups.Where(colorGroup => colorGroup.Included.Contains(modifierName)))
				return colorGroup.Color;

			return Color.white;
		}

		const string DefaultName = "DefaultModifierSettings";

		/// <summary> Returns global default settings asset. You need to setup it manually, or it will try to load it from resources (if exist). </summary>
		public static DefaultUnityModifierSettings Get()
		{
			if (!instance)
			{
				Debug.LogWarning($"No Default Unity Modifier Settings was setup by using {nameof(Setup)} method! Trying to find it in Resources...");

				instance = Resources.Load<DefaultUnityModifierSettings>(DefaultName);
				
				if (instance)
					instance.Init();
				else 
					Debug.LogWarning("No Default Unity Modifier Settings asset exist!");
			}

			return instance;
		}

#if UNITY_EDITOR
		public static bool TryGetEditor(out DefaultUnityModifierSettings result)
		{
			if (instance)
			{
				result = instance;
				return true;
			}

			var assets = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(DefaultUnityModifierSettings)}");

			if (assets.Length > 0)
			{
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]);
				result = UnityEditor.AssetDatabase.LoadAssetAtPath<DefaultUnityModifierSettings>(path);
				instance = result;
				return true;
			}

			result = default;
			return false;
		}
#endif
	}

	[Serializable]
	public class ColorGroup
	{
		public List<string> Included = new List<string>();
		public Color Color = Color.white;
	}
}