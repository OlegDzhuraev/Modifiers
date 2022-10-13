using InsaneOne.Modifiers;
using UnityEngine;

namespace Example.Sources
{
	public class Damager : MonoBehaviour
	{
		[SerializeField] Character charToDamage;
		[SerializeField] Modifier data;

		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				var damage = data.GetValue(ModifierType.Damage);
				var isCritical = false;

				if (data.IsTrue(ModifierType.CriticalChance))
				{
					var criticalChance = data.GetValue(ModifierType.CriticalChance);
					isCritical = Random.Range(0f, 1f) < criticalChance;
				}

				if (isCritical)
				{
					damage *= 2f;
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
}