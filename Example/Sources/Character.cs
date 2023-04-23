using System;
using UnityEngine;

namespace InsaneOne.Modifiers.Example
{
	// Alternative variation without usage of the ready-made Modifable component.
	// This is more like classic Monobehaviour approach, here Modifiers used just as storage of basic character stats.
	// For more "interesting" example check class CharacterModifable.
	public class Character : MonoBehaviour
	{
		public event Action<float> HealthChanged;

		[SerializeField] UnityModifier data;

		public float Health { get; private set; }

		float timer;

		void Start()
		{
			SetHealth(data.GetValue(ModType.MaxHealth));
		}

		void Update()
		{
			timer -= Time.deltaTime;

			if (timer > 0)
				return;

			if (Health < data.GetValue(ModType.MaxHealth))
			{
				var regen = data.GetValue(ModType.Regeneration);
				SetHealth(Health + regen);

				GameStateLog.Log($"<color=#99ff99>Character regenerated {regen} HP</color>");
			}

			timer = 1f;
		}

		public void TakeDamage(float value)
		{
			SetHealth(Health - value / data.GetValue(ModType.Defense));
		}

		void SetHealth(float value)
		{
			Health = Mathf.Clamp(value, 0f, data.GetValue(ModType.MaxHealth));

			HealthChanged?.Invoke(Health);
		}
	}
}