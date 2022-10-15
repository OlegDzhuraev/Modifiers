using System;
using UnityEngine;
using UnityEngine.UI;

namespace InsaneOne.Modifiers.Example
{
	public class ExampleOutput : MonoBehaviour
	{
		[SerializeField] Text charHealthText;
		[SerializeField] Text log;
		[SerializeField] Character character;

		void Awake()
		{
			character.HealthChanged += OnHealthChanged;
			GameStateLog.MessageReceived += OnLogMessageReceived;
		}

		void OnDestroy()
		{
			character.HealthChanged -= OnHealthChanged;
			GameStateLog.MessageReceived -= OnLogMessageReceived;
		}

		void OnLogMessageReceived(string text)
		{
			log.text = $"{text}\n{log.text}";
		}

		void OnHealthChanged(float health)
		{
			charHealthText.text = $"Health: {Math.Round(health, 2)}";
		}
	}
}