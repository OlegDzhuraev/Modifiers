using NUnit.Framework;

namespace InsaneOne.Modifiers.Tests
{
	public class ModificatorObserverTests
	{
		[Test]
		public void SubTo_NotifyValueChange_InvokesCallbackWithValue()
		{
			var observer = new ModificatorObserver();
			float? received = null;

			observer.SubTo("Health", value => received = value);
			observer.NotifyValueChange("Health", 42);

			Assert.AreEqual(42f, received);
		}

		[Test]
		public void NotifyValueChange_UnrelatedType_DoesNotInvoke()
		{
			var observer = new ModificatorObserver();
			var invoked = false;

			observer.SubTo("Health", _ => invoked = true);
			observer.NotifyValueChange("Damage", 10);

			Assert.IsFalse(invoked);
		}

		[Test]
		public void UnsubFrom_StopsReceivingNotifications()
		{
			var observer = new ModificatorObserver();
			var callCount = 0;
			void Callback(float value) => callCount++;

			observer.SubTo("Health", Callback);
			observer.NotifyValueChange("Health", 1);
			observer.UnsubFrom("Health", Callback);
			observer.NotifyValueChange("Health", 2);

			Assert.AreEqual(1, callCount);
		}

		[Test]
		public void SubTo_MultipleSubscribers_AllInvoked()
		{
			var observer = new ModificatorObserver();
			var firstCalled = false;
			var secondCalled = false;

			observer.SubTo("Health", _ => firstCalled = true);
			observer.SubTo("Health", _ => secondCalled = true);
			observer.NotifyValueChange("Health", 1);

			Assert.IsTrue(firstCalled);
			Assert.IsTrue(secondCalled);
		}

		[Test]
		public void NotifyValueChange_NoSubscribers_DoesNotThrow()
		{
			var observer = new ModificatorObserver();

			Assert.DoesNotThrow(() => observer.NotifyValueChange("Health", 1));
		}
	}
}
