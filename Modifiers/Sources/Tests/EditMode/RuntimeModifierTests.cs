using NUnit.Framework;
using InsaneOne.Modifiers.Processors;

namespace InsaneOne.Modifiers.Tests
{
	public class RuntimeModifierTests
	{
		static Modifier MakeModifier(params (string type, float value)[] parameters)
		{
			var modifier = new Modifier();

			foreach (var (type, value) in parameters)
				modifier.SetParamValue(type, value);

			return modifier;
		}

		[Test]
		public void Add_SingleModifier_SumsIntoValue()
		{
			var runtime = new RuntimeModifier(null);

			runtime.Add(MakeModifier(("Health", 10)));

			Assert.AreEqual(10, runtime.GetRawValue("Health"));
		}

		[Test]
		public void Add_MultipleModifiers_AccumulatesSameType()
		{
			var runtime = new RuntimeModifier(null);

			runtime.Add(MakeModifier(("Health", 10)));
			runtime.Add(MakeModifier(("Health", 5)));

			Assert.AreEqual(15, runtime.GetRawValue("Health"));
		}

		[Test]
		public void Remove_ExistingModifier_SubtractsValue()
		{
			var runtime = new RuntimeModifier(null);
			var modifier = MakeModifier(("Health", 10));
			runtime.Add(modifier);

			runtime.Remove(modifier);

			Assert.AreEqual(0, runtime.GetRawValue("Health"));
		}

		[Test]
		public void Remove_ModifierWithParamTypeNeverAdded_DoesNotThrowAndIsIgnored()
		{
			// Regression test: Remove() used to index the internal dictionary directly and throw
			// KeyNotFoundException for a param type that was never Add-ed (e.g. appended to the modifier's
			// Parameters after it was already applied, via SetParamValue).
			var runtime = new RuntimeModifier(null);
			var modifier = MakeModifier(("NeverAdded", 5));

			Assert.DoesNotThrow(() => runtime.Remove(modifier));
			Assert.IsFalse(runtime.TryGetValue("NeverAdded", out _));
		}

		[Test]
		public void SetValue_OverridesRawValue()
		{
			var runtime = new RuntimeModifier(null);
			runtime.Add(MakeModifier(("Health", 10)));

			runtime.SetValue("Health", 999);

			Assert.AreEqual(999, runtime.GetRawValue("Health"));
		}

		[Test]
		public void AddValue_NewType_SetsValue()
		{
			var runtime = new RuntimeModifier(null);

			runtime.AddValue("Health", 7);

			Assert.AreEqual(7, runtime.GetRawValue("Health"));
		}

		[Test]
		public void AddValue_ExistingType_Accumulates()
		{
			var runtime = new RuntimeModifier(null);
			runtime.AddValue("Health", 7);

			runtime.AddValue("Health", 3);

			Assert.AreEqual(10, runtime.GetRawValue("Health"));
		}

		[Test]
		public void TryGetValue_MissingType_ReturnsFalseAndZero()
		{
			var runtime = new RuntimeModifier(null);

			var found = runtime.TryGetValue("Health", out var value);

			Assert.IsFalse(found);
			Assert.AreEqual(0, value);
		}

		[TestCase(1f, true)]
		[TestCase(0f, false)]
		[TestCase(-1f, false)]
		public void IsTrue_ReflectsRawValueSign(float value, bool expected)
		{
			var runtime = new RuntimeModifier(null);
			runtime.SetValue("Tag", value);

			Assert.AreEqual(expected, runtime.IsTrue("Tag"));
		}

		[Test]
		public void GetValue_UsesProcessor_WhenSettingsProvided()
		{
			var settings = new StubModifiersSettings();
			settings.Set("Health", new ModifierParamData
			{
				Name = "Health",
				Processors = new ModifierProcessor[] { new ClampProcessor { MinValue = 0, MaxValue = 100 } },
			});
			var runtime = new RuntimeModifier(settings);

			runtime.SetValue("Health", 500);

			Assert.AreEqual(500, runtime.GetRawValue("Health"));
			Assert.AreEqual(100, runtime.GetValue("Health"));
		}

		[Test]
		public void SetValue_NotifiesObserver_WithProcessedValue()
		{
			var runtime = new RuntimeModifier(null);
			float? notified = null;
			runtime.Observer.SubTo("Health", value => notified = value);

			runtime.SetValue("Health", 42);

			Assert.AreEqual(42f, notified);
		}
	}
}
