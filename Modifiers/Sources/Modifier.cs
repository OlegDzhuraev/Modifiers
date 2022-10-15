using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "NewModifier", menuName = "Modifiers/New modifier")]
	public class Modifier : ScriptableObject
	{
		[SerializeField] List<ModifierParam> parameters = new List<ModifierParam>();

		public float GetValue(ModifierType type) => GetParam(type).Value;
		public bool IsTrue(ModifierType type) => GetParam(type).Value > 0;

		ModifierParam GetParam(ModifierType type)
		{
			foreach (var modifierParam in parameters)
				if (modifierParam.Type == type)
					return modifierParam;

			var defaultValues = DefaultModifierSettings.Get();

			if (defaultValues)
				return defaultValues.GetParam(type);
			
			throw new NullReferenceException($"No {type} param found.");
		}
	}
}