using UnityEngine;

namespace InsaneOne.Modifiers.Example
{
	public class Damager : MonoBehaviour
	{
		const float CriticalDamageMultiplier = 2f;
		
		[SerializeField] Character charToDamage;
		[SerializeField] Modifier data;

		void Update()
		{
			if (!Input.GetMouseButtonDown(0))
				return;
			
			var damage = data.GetValue(ModifierType.Damage);
			var isCritical = false;

			if (data.IsTrue(ModifierType.CriticalChance))
			{
				var criticalChance = data.GetValue(ModifierType.CriticalChance);
				isCritical = Random.Range(0f, 1f) < criticalChance;
			}

			if (isCritical)
			{
				damage *= CriticalDamageMultiplier;
				GameStateLog.Log($"<color=red>Damager deals critical damage {damage}</color>");
			}
			else
			{
				GameStateLog.Log($"<color=orange>Damager deals damage {damage}</color>");
			}

			charToDamage.TakeDamage(damage);
		}
	}
}