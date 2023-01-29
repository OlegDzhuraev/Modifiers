using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "NewModifier", menuName = "Modifiers/New modifier")]
	public class Modifier : ScriptableObject
	{
		[SerializeField] List<ModifierParam> parameters = new List<ModifierParam>();

		public float GetValue(ModType type) => GetParam(type).Value;
		public bool IsTrue(ModType type) => GetParam(type).Value > 0;

		internal List<ModifierParam> GetAllValues() => parameters;

		ModifierParam GetParam(ModType type)
		{
			foreach (var modifierParam in parameters)
				if (modifierParam.Type == type)
					return modifierParam;

			if (GetType() != typeof(DefaultModifierSettings))
			{
				var defaultValues = DefaultModifierSettings.Get();

				if (defaultValues)
					return defaultValues.GetParam(type);
			}

			throw new NullReferenceException($"No {type} param found.");
		}
	}
}