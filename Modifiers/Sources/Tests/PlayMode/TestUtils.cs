using System.Reflection;
using UnityEngine;
using InsaneOne.Modifiers.Buffs;

namespace InsaneOne.Modifiers.Tests
{
	/// <summary> Test-only helpers for building UnityModifier/Buff ScriptableObject instances whose backing
	/// fields are private [SerializeField]s with no public setters. Uses reflection instead of the
	/// UNITY_EDITOR-only EditorSetModifier API, so these tests behave the same whether run inside the Editor
	/// or as a standalone Play Mode test player. </summary>
	public static class TestUtils
	{
		public static UnityModifier CreateUnityModifier(string name, params (string type, float value)[] parameters)
		{
			var asset = ScriptableObject.CreateInstance<UnityModifier>();
			asset.name = name;

			var modifier = new Modifier { Name = name };
			foreach (var (type, value) in parameters)
				modifier.SetParamValue(type, value);

			SetPrivateField(asset, "modifier", modifier);

			return asset;
		}

		public static Buff CreateBuff(UnityModifier modifier, float lifeTime, int maxStacks)
		{
			var buff = ScriptableObject.CreateInstance<Buff>();

			SetPrivateField(buff, "modifier", modifier);
			SetPrivateField(buff, "lifeTime", lifeTime);
			SetPrivateField(buff, "maxStacks", maxStacks);

			return buff;
		}

		static void SetPrivateField(object target, string fieldName, object value)
		{
			var field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			field.SetValue(target, value);
		}
	}
}
