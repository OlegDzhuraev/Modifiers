using UnityEngine;

namespace InsaneOne.Modifiers.Example
{
	public class HealthUtility
	{
		public static void SetHealth(GameObject target, float value)
		{
			var health = Mathf.Clamp(value, 0f, target.GetModifierValue(ModType.MaxHealth));
			target.SetModifierValue(ModType.Health, health);
		}
	}
}