using System;
using UnityEngine;
using UnityEngine.UI;

namespace InsaneOne.Modifiers.Example
{
	public class ExampleOutput : MonoBehaviour
	{
		[SerializeField] Text charHealthText;
		[SerializeField] Text log;
		[SerializeField] GameObject character;

		bool isModifable;
		
		void Awake()
		{
			isModifable = character.GetComponent<Modifable>();
			
			if (isModifable)
				character.SubToModifier(ModType.Health, OnHealthChanged);
			else
				character.GetComponent<Character>().HealthChanged += OnHealthChanged;
			
			GameStateLog.MessageReceived += OnLogMessageReceived;
		}

		void OnDestroy()
		{
			if (character)
			{
				if (isModifable)
					character.UnsubFromModifier(ModType.Health, OnHealthChanged);
				else
					character.GetComponent<Character>().HealthChanged -= OnHealthChanged;
			}

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