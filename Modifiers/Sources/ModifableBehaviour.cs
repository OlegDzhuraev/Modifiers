using System;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	/// <summary> Can be used instead of MonoBehaviour to reduce code size. </summary>
	[RequireComponent(typeof(Modifable))]
	public class ModifableBehaviour : MonoBehaviour
	{
		public void SetValue(ModType type, float value) => gameObject.SetModifierValue(type, value);
		public void AddValue(ModType type, float value) => gameObject.AddModifierValue(type, value);
		public float GetValue(ModType type) => gameObject.GetModifierValue(type);
		public bool IsValueTrue(ModType type) => GetValue(type) > 0;
		
		public void AddTag(params ModType[] tags) => gameObject.AddTag(tags);
		public void AddTagOnce(params ModType[] tags) => gameObject.AddTagOnce(tags);
		public void RemoveTag(params ModType[] tags) => gameObject.RemoveTag(tags);
		public bool HasTag(ModType tag) => gameObject.HasTag(tag);
		public bool HasAnyTag(params ModType[] tags) => gameObject.HasAnyTags(tags);
		public bool HasAllTag(params ModType[] tags) => gameObject.HasAllTags(tags);

		public void AddModifier(Modifier modifier) => gameObject.AddModifier(modifier);
		public void RemoveModifier(Modifier modifier) => gameObject.RemoveModifier(modifier);
		
		public void SubTo(ModType type, Action<float> action) => gameObject.SubToModifier(type, action);
		public void UnsubFrom(ModType type, Action<float> action) => gameObject.UnsubFromModifier(type, action);
	}
}