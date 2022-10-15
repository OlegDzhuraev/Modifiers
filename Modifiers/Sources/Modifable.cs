using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	public class Modifable : MonoBehaviour
	{
		public event Action WasChanged;
		
		[SerializeField] Modifier defaultModifier;
		
		readonly List<Modifier> modifiers = new List<Modifier>();
		
		void Awake()
		{
			if (defaultModifier)
				Add(defaultModifier);
		}

		public void Add(Modifier modifier)
		{
			modifiers.Add(modifier);
			WasChanged?.Invoke();
		}

		public void Remove(Modifier modifier)
		{
			modifiers.Remove(modifier);
			WasChanged?.Invoke();
		}
		
		public float GetValue(ModifierType type)
		{
			var result = 0f;
			
			foreach (var modifier in modifiers)
				result += modifier.GetValue(type);

			return result;
		}

		public bool IsTrue(ModifierType type)
		{
			foreach (var modifier in modifiers)
				if (modifier.IsTrue(type))
					return true;

			return false;
		}
	}
}