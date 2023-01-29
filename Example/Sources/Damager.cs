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
			
			var damage = data.GetValue(ModType.Damage);
			var isCritical = false;

			if (data.IsTrue(ModType.CriticalChance))
			{
				var criticalChance = data.GetValue(ModType.CriticalChance);
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