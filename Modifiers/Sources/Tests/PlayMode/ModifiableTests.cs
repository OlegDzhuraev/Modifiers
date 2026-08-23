using NUnit.Framework;
using UnityEngine;

namespace InsaneOne.Modifiers.Tests
{
	public class ModifiableTests
	{
		GameObject go;
		Modifiable modifiable;

		[SetUp]
		public void SetUp()
		{
			go = new GameObject(nameof(ModifiableTests));
			modifiable = go.AddComponent<Modifiable>();
		}

		[TearDown]
		public void TearDown()
		{
			if (go)
				Object.DestroyImmediate(go);
		}

		[Test]
		public void Add_SingleModifier_AddsValueToModifiable()
		{
			modifiable.Add(TestUtils.CreateUnityModifier("Buff", ("Health", 10)));

			Assert.AreEqual(10, modifiable.GetValue("Health"));
		}

		[Test]
		public void Add_MultipleModifiers_SumsValues()
		{
			modifiable.Add(TestUtils.CreateUnityModifier("A", ("Health", 10)));
			modifiable.Add(TestUtils.CreateUnityModifier("B", ("Health", 5)));

			Assert.AreEqual(15, modifiable.GetValue("Health"));
		}

		[Test]
		public void Remove_RemovesAppliedValue()
		{
			var modifier = TestUtils.CreateUnityModifier("Buff", ("Health", 10));
			modifiable.Add(modifier);

			modifiable.Remove(modifier);

			Assert.AreEqual(0, modifiable.GetValue("Health"));
		}

		[Test]
		public void Remove_ModifierThatWasNeverAdded_IsNoOp()
		{
			var modifier = TestUtils.CreateUnityModifier("Buff", ("Health", 10));

			Assert.DoesNotThrow(() => modifiable.Remove(modifier));
			Assert.AreEqual(0, modifiable.GetValue("Health"));
		}

		[Test]
		public void SetValue_OverridesCurrentValue()
		{
			modifiable.Add(TestUtils.CreateUnityModifier("Buff", ("Health", 10)));

			modifiable.SetValue("Health", 999);

			Assert.AreEqual(999, modifiable.GetValue("Health"));
		}

		[Test]
		public void AddValue_AccumulatesOnTopOfCurrentValue()
		{
			modifiable.SetValue("Health", 10);

			modifiable.AddValue("Health", 5);

			Assert.AreEqual(15, modifiable.GetValue("Health"));
		}

		[Test]
		public void WasChanged_FiresOnAddAndRemove()
		{
			var fired = 0;
			modifiable.WasChanged += () => fired++;
			var modifier = TestUtils.CreateUnityModifier("Buff", ("Health", 10));

			modifiable.Add(modifier);
			modifiable.Remove(modifier);

			Assert.AreEqual(2, fired);
		}

		[Test]
		public void SubTo_ReceivesNotificationOnValueChange()
		{
			float? received = null;
			modifiable.SubTo("Health", value => received = value);

			modifiable.SetValue("Health", 42);

			Assert.AreEqual(42f, received);
		}

		[Test]
		public void UnsubFrom_StopsReceivingNotifications()
		{
			var callCount = 0;
			void Callback(float value) => callCount++;
			modifiable.SubTo("Health", Callback);

			modifiable.SetValue("Health", 1);
			modifiable.UnsubFrom("Health", Callback);
			modifiable.SetValue("Health", 2);

			Assert.AreEqual(1, callCount);
		}

		[TestCase(1f, true)]
		[TestCase(0f, false)]
		[TestCase(-1f, false)]
		public void IsTrue_ReflectsValueSign(float value, bool expected)
		{
			modifiable.SetValue("Tag", value);

			Assert.AreEqual(expected, modifiable.IsTrue("Tag"));
		}

		[Test]
		public void GetRawValue_ReturnsUnprocessedSum()
		{
			modifiable.Add(TestUtils.CreateUnityModifier("Buff", ("Health", 10)));

			Assert.AreEqual(10, modifiable.GetRawValue("Health"));
		}
	}
}
