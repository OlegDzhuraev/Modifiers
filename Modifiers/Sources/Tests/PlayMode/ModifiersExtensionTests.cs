#if UNITY_5_3_OR_NEWER && INSANEONE_MODIFIERS_UNITY_EXTENSION
using NUnit.Framework;
using UnityEngine;

namespace InsaneOne.Modifiers.Tests
{
	public class ModifiersExtensionTests
	{
		GameObject go;

		[SetUp]
		public void SetUp() => go = new GameObject(nameof(ModifiersExtensionTests));

		[TearDown]
		public void TearDown()
		{
			if (go)
				Object.DestroyImmediate(go);
		}

		[Test]
		public void AddModifier_WithoutExistingModifiableComponent_AddsOneAutomatically()
		{
			Assert.IsNull(go.GetComponent<Modifiable>());

			go.AddModifier(TestUtils.CreateUnityModifier("Buff", ("Health", 10)));

			Assert.IsNotNull(go.GetComponent<Modifiable>());
			Assert.AreEqual(10, go.GetModifierValue("Health"));
		}

		[Test]
		public void AddModifier_PlainModifierOverload_AppliesValue()
		{
			var modifier = new Modifier();
			modifier.SetParamValue("Health", 7);

			go.AddModifier(modifier);

			Assert.AreEqual(7, go.GetModifierValue("Health"));
		}

		[Test]
		public void RemoveModifier_RemovesAppliedValue()
		{
			var modifier = TestUtils.CreateUnityModifier("Buff", ("Health", 10));
			go.AddModifier(modifier);

			go.RemoveModifier(modifier);

			Assert.AreEqual(0, go.GetModifierValue("Health"));
		}

		[Test]
		public void GetIntModifierValue_TruncatesToInt()
		{
			go.SetModifierValue("Health", 9.7f);

			Assert.AreEqual(9, go.GetIntModifierValue("Health"));
		}

		[Test]
		public void IsModifierValueTrue_ReflectsSign()
		{
			go.SetModifierValue("Flag", 1);
			Assert.IsTrue(go.IsModifierValueTrue("Flag"));

			go.SetModifierValue("Flag", 0);
			Assert.IsFalse(go.IsModifierValueTrue("Flag"));
		}

		[Test]
		public void AddModifierValue_AccumulatesOnTopOfCurrent()
		{
			go.SetModifierValue("Health", 10);

			go.AddModifierValue("Health", 5);

			Assert.AreEqual(15, go.GetModifierValue("Health"));
		}

		[Test]
		public void SubToModifier_ReceivesNotification()
		{
			float? received = null;
			go.SubToModifier("Health", value => received = value);

			go.SetModifierValue("Health", 42);

			Assert.AreEqual(42f, received);
		}

		[Test]
		public void UnsubFromModifier_StopsReceivingNotifications()
		{
			var callCount = 0;
			void Callback(float value) => callCount++;
			go.SubToModifier("Health", Callback);

			go.SetModifierValue("Health", 1);
			go.UnsubFromModifier("Health", Callback);
			go.SetModifierValue("Health", 2);

			Assert.AreEqual(1, callCount);
		}

		[Test]
		public void AddTag_SetsValueToOne()
		{
			go.AddTag("Poisoned");

			Assert.IsTrue(go.HasTag("Poisoned"));
		}

		[Test]
		public void AddTagOnce_DoesNotAccumulateOnRepeatedCalls()
		{
			go.AddTagOnce("Poisoned");
			go.AddTagOnce("Poisoned");

			Assert.AreEqual(1, go.GetModifierValue("Poisoned"));
		}

		[Test]
		public void RemoveTag_ClearsTag()
		{
			go.AddTag("Poisoned");

			go.RemoveTag("Poisoned");

			Assert.IsFalse(go.HasTag("Poisoned"));
		}

		[Test]
		public void HasAnyTags_TrueIfAtLeastOnePresent()
		{
			go.AddTag("A");

			Assert.IsTrue(go.HasAnyTags("A", "B"));
			Assert.IsFalse(go.HasAnyTags("B", "C"));
		}

		[Test]
		public void HasAllTags_TrueOnlyIfEveryTagPresent()
		{
			go.AddTag("A");
			go.AddTag("B");

			Assert.IsTrue(go.HasAllTags("A", "B"));

			go.RemoveTag("B");

			Assert.IsFalse(go.HasAllTags("A", "B"));
		}

		[Test]
		public void CompareValues_TrueOnlyWhenBothGameObjectsHaveSameValue()
		{
			var other = new GameObject("Other");

			try
			{
				go.SetModifierValue("Health", 10);
				other.SetModifierValue("Health", 10);

				Assert.IsTrue(go.CompareValues("Health", other));

				other.SetModifierValue("Health", 5);

				Assert.IsFalse(go.CompareValues("Health", other));
			}
			finally
			{
				Object.DestroyImmediate(other);
			}
		}
	}
}
#endif
