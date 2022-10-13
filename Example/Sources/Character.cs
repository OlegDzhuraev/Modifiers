using System;
using InsaneOne.Modifiers;
using UnityEngine;

namespace Example.Sources
{
	public class Character : MonoBehaviour
	{
		public event Action<float> HealthChanged;

		[SerializeField] Modifier data;

		public float Health { get; private set; }

		float timer;

		void Start()
		{
			SetHealth(data.GetValue(ModifierType.MaxHealth));
		}

		void Update()
		{
			timer -= Time.deltaTime;

			if (timer > 0)
				return;

			if (Health < data.GetValue(ModifierType.MaxHealth))
			{
				var regenValue = data.GetValue(ModifierType.Regeneration);
				SetHealth(Health + regenValue);

				GameStateLog.Log($"<color=#99ff99>Character regenerated {regenValue} HP</color>");
			}

			timer = 1f;
		}

		public void TakeDamage(float value)
		{
			SetHealth(Health - value / data.GetValue(ModifierType.Defense));
		}

		void SetHealth(float value)
		{
			Health = Mathf.Clamp(value, 0f, data.GetValue(ModifierType.MaxHealth));

			HealthChanged?.Invoke(Health);
		}
	}
}