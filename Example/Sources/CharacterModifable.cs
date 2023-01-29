using UnityEngine;

namespace InsaneOne.Modifiers.Example
{
	// Alternative variation with usage of the ready-made Modifable component.
	// In this variation there is no inner variable named health, it is moved to an whole object scope.
	// It allows us also remove HealthChanged event, since other code can subscribe to changes of the Modifable component and track actual health value.
	public class CharacterModifable : ModifableBehaviour
	{
		float timer;
		
		void Start()
		{
			SetHealth(GetValue(ModType.MaxHealth));
		}
		
		void Update()
		{
			timer -= Time.deltaTime;

			if (timer > 0)
				return;

			var health = GetValue(ModType.Health);
			if (health < GetValue(ModType.MaxHealth))
			{
				var regen = GetValue(ModType.Regeneration);
				SetHealth(health + regen);

				GameStateLog.Log($"<color=#99ff99>Character regenerated {regen} HP</color>");
			}

			timer = 1f;
		}

		public void TakeDamage(float value)
		{
			SetHealth(GetValue(ModType.Health) - value / GetValue(ModType.Defense));
		}

		void SetHealth(float value)
		{
			var health = Mathf.Clamp(value, 0f, GetValue(ModType.MaxHealth));
			SetValue(ModType.Health, health);
		}
	}
}