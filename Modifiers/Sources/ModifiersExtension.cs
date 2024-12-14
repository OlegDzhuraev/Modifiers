#if INSANEONE_MODIFIERS_EXTENSION

using System;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	public static class ModifiersExtension
	{
		static Modifiable Get(GameObject go)
		{
			if (!Modifiable.all.TryGetValue(go, out var modifiable))
				modifiable = go.TryGetComponent<Modifiable>(out var comp) ? comp : go.AddComponent<Modifiable>();

			return modifiable;
		}
		
		public static void AddModifier(this GameObject go, UnityModifier modifier) => Get(go).Add(modifier);
		public static void AddModifier(this GameObject go, Modifier modifier) => Get(go).Add(modifier);
		
		public static void RemoveModifier(this GameObject go, UnityModifier modifier) => Get(go).Remove(modifier);
		public static void RemoveModifier(this GameObject go, Modifier modifier) => Get(go).Remove(modifier);
		
		public static float GetModifierValue(this GameObject go, string type) => Get(go).GetValue(type);
		public static float GetModifierRawValue(this GameObject go, string type) => Get(go).GetRawValue(type);
		public static int GetIntModifierValue(this GameObject go, string type) => (int)go.GetModifierValue(type);
		public static bool IsModifierValueTrue(this GameObject go, string type) => Get(go).GetValue(type) > 0;
		public static void AddModifierValue(this GameObject go, string type, float value) => Get(go).AddValue(type, value);
		public static void SetModifierValue(this GameObject go, string type, float value) => Get(go).SetValue(type, value);
		
		public static void SubToModifier(this GameObject go, string type, Action<float> action) => Get(go).SubTo(type, action);
		public static void UnsubFromModifier(this GameObject go, string type, Action<float> action) => Get(go).UnsubFrom(type, action);

		public static void AddTag(this GameObject go, params string[] tags)
		{
			var mf = Get(go);

			foreach (var tag in tags)
				mf.AddValue(tag, 1);
		}

		public static void AddTagOnce(this GameObject go, params string[] tags)
		{
			var mf = Get(go);
			
			foreach (var tag in tags)
				if (mf.GetValue(tag) < 1)
					mf.AddValue(tag, 1);
		}

		public static void RemoveTag(this GameObject go, params string[] tags)
		{
			var mf = Get(go);
			
			foreach (var tag in tags)
				if (mf.GetValue(tag) > 0)
					mf.AddValue(tag, -1);
		}

		public static bool HasTag(this GameObject go, string tag) => Get(go).IsTrue(tag);
		
		public static bool HasAnyTags(this GameObject go, params string[] tags)
		{
			var mf = Get(go);
			
			for (var i = 0; i < tags.Length; i++)
				if (mf.IsTrue(tags[i]))
					return true;

			return false;
		}

		public static bool HasAllTags(this GameObject go, params string[] tags)
		{
			var mf = Get(go);

			for (var i = 0; i < tags.Length; i++)
				if (!mf.IsTrue(tags[i]))
					return false;

			return true;
		}

		public static bool CompareValues(this GameObject go, string type, GameObject other) 
			=> Mathf.Approximately(go.GetModifierValue(type), other.GetModifierValue(type));
	}
}

#endif