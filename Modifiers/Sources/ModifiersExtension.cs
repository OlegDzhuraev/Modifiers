using System;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	public static class ModifiersExtension
	{
		static Modifable Get(GameObject go)
		{
			if (!Modifable.all.TryGetValue(go, out var modifable))
				modifable = go.TryGetComponent<Modifable>(out var comp) ? comp : go.AddComponent<Modifable>();
			
			return modifable;
		}
		
		public static void AddModifier(this GameObject go, Modifier modifier) => Get(go).Add(modifier);
		public static void RemoveModifier(this GameObject go, Modifier modifier) => Get(go).Remove(modifier);
		
		public static float GetModifierValue(this GameObject go, ModType type) => Get(go).GetValue(type);
		public static int GetIntModifierValue(this GameObject go, ModType type) => (int)go.GetModifierValue(type);
		public static bool IsModifierValueTrue(this GameObject go, ModType type) => Get(go).GetValue(type) > 0;
		public static void AddModifierValue(this GameObject go, ModType type, float value) => Get(go).AddValue(type, value);
		public static void SetModifierValue(this GameObject go, ModType type, float value) => Get(go).SetValue(type, value);
		
		public static void SubToModifier(this GameObject go, ModType type, Action<float> action) => Get(go).SubTo(type, action);
		public static void UnsubFromModifier(this GameObject go, ModType type, Action<float> action) => Get(go).UnsubFrom(type, action);

		public static void AddTag(this GameObject go, params ModType[] tags)
		{
			var mf = Get(go);

			foreach (var tag in tags)
				mf.AddValue(tag, 1);
		}

		public static void AddTagOnce(this GameObject go, params ModType[] tags)
		{
			var mf = Get(go);
			
			foreach (var tag in tags)
				if (mf.GetValue(tag) < 1)
					mf.AddValue(tag, 1);
		}

		public static void RemoveTag(this GameObject go, params ModType[] tags)
		{
			var mf = Get(go);
			
			foreach (var tag in tags)
				if (mf.GetValue(tag) > 0)
					mf.AddValue(tag, -1);
		}

		public static bool HasTag(this GameObject go, ModType tag) => Get(go).IsTrue(tag);
		
		public static bool HasAnyTags(this GameObject go, params ModType[] tags)
		{
			var mf = Get(go);
			
			for (var i = 0; i < tags.Length; i++)
				if (mf.IsTrue(tags[i]))
					return true;

			return false;
		}

		public static bool HasAllTags(this GameObject go, params ModType[] tags)
		{
			var mf = Get(go);

			for (var i = 0; i < tags.Length; i++)
				if (!mf.IsTrue(tags[i]))
					return false;

			return true;
		}

		public static bool CompareValues(this GameObject go, ModType type, GameObject other) 
			=> Mathf.Approximately(go.GetModifierValue(type), other.GetModifierValue(type));
	}
}