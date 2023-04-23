using UnityEngine;
using Random = UnityEngine.Random;

namespace InsaneOne.Modifiers.Example
{
	// it contains example of working with two approaches:
	// 1. when you use modifiers without Modifable component. It requires link to a Character class, it can be annoying in the big projects.
	// 2. when you use modifiers WITH Modifable component. So, now we need only link to the GameObject to change its stats.
	// It can be very easy to work in this approach, since now you can create static Utilities,
	// which can handle changes of the object like HealthUtility class.
	// It is don't know anything except GameObject and Modifable classes.
	public class Damager : MonoBehaviour
	{
		const float CriticalDamageMultiplier = 2f;
		
		[SerializeField] GameObject target;
		[SerializeField] UnityModifier data;

		Character charToDamageComp;

		bool isModifable;
		
		void Start()
		{
			isModifable = target.GetComponent<Modifable>();
			
			if (!isModifable)
				charToDamageComp = target.GetComponent<Character>();
		}

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

			if (isModifable)
				DoTargetDamage(damage);
			else
				charToDamageComp.TakeDamage(damage);
		}
		
		public void DoTargetDamage(float value)
		{
			var health = target.GetModifierValue(ModType.Health);
			var defense = target.GetModifierValue(ModType.Defense);

			HealthUtility.SetHealth(target, health - value / defense);
		}
	}
}