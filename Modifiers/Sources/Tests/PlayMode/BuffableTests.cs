#if UNITY_5_3_OR_NEWER && INSANEONE_MODIFIERS_UNITY_EXTENSION
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using InsaneOne.Modifiers.Buffs;

namespace InsaneOne.Modifiers.Tests
{
	public class BuffableTests
	{
		GameObject go;
		Modifiable modifiable;
		Buffable buffable;

		[SetUp]
		public void SetUp()
		{
			go = new GameObject(nameof(BuffableTests));
			modifiable = go.AddComponent<Modifiable>();
			buffable = go.AddComponent<Buffable>();
			buffable.Modifiable = modifiable;
		}

		[TearDown]
		public void TearDown()
		{
			if (go)
				Object.DestroyImmediate(go);
		}

		static Buff CreateBuff(string name, float value, float lifeTime = 0, int maxStacks = 5)
		{
			var unityModifier = TestUtils.CreateUnityModifier(name, ("Health", value));
			return TestUtils.CreateBuff(unityModifier, lifeTime, maxStacks);
		}

		[Test]
		public void AddBuff_AppliesModifierValue()
		{
			buffable.AddBuff(CreateBuff("Regen", 10));

			Assert.AreEqual(10, modifiable.GetValue("Health"));
		}

		[Test]
		public void AddBuff_MultipleStacks_AccumulatesValue()
		{
			var buff = CreateBuff("Regen", 10, maxStacks: 3);

			buffable.AddBuff(buff);
			buffable.AddBuff(buff);

			Assert.AreEqual(20, modifiable.GetValue("Health"));
		}

		[Test]
		public void AddBuff_BeyondMaxStacks_IsIgnored()
		{
			var buff = CreateBuff("Regen", 10, maxStacks: 1);

			buffable.AddBuff(buff);
			buffable.AddBuff(buff);

			Assert.AreEqual(10, modifiable.GetValue("Health"));
		}

		[Test]
		public void AddBuff_FiresBuffAdded()
		{
			var fired = 0;
			buffable.BuffAdded += _ => fired++;

			buffable.AddBuff(CreateBuff("Regen", 10));

			Assert.AreEqual(1, fired);
		}

		[Test]
		public void RemoveBuff_RemovesOneStackValue()
		{
			var buff = CreateBuff("Regen", 10, maxStacks: 3);
			buffable.AddBuff(buff);
			buffable.AddBuff(buff);

			buffable.RemoveBuff(buff);

			Assert.AreEqual(10, modifiable.GetValue("Health"));
		}

		[UnityTest]
		public IEnumerator Timer_ExpiresAfterLifeTime_RemovesBuffAutomatically()
		{
			buffable.AddBuff(CreateBuff("Poison", -10, lifeTime: 0.1f));
			Assert.AreEqual(-10, modifiable.GetValue("Health"));

			yield return new WaitForSeconds(0.3f);

			Assert.AreEqual(0, modifiable.GetValue("Health"));
		}

		[UnityTest]
		public IEnumerator RemoveBuff_ManualRemovalBeforeTimerExpires_DoesNotFireExtraRemoval()
		{
			// Regression test: RemoveBuff() used to leave a stale BuffTimer behind when a stack was removed
			// manually before its own timer expired. That orphaned timer would later still fire, causing an
			// extra spurious RemoveBuff/BuffRemoved call beyond the two real stacks that were actually added.
			var buff = CreateBuff("Poison", -10, lifeTime: 0.1f, maxStacks: 3);
			var removedEvents = new List<Buff>();
			buffable.BuffRemoved += b => removedEvents.Add(b);

			buffable.AddBuff(buff);
			buffable.AddBuff(buff);
			Assert.AreEqual(-20, modifiable.GetValue("Health"));

			buffable.RemoveBuff(buff); // manual removal of one stack, well before the 0.1s timer expires

			yield return new WaitForSeconds(0.3f); // let the remaining stack's timer expire naturally

			Assert.AreEqual(0, modifiable.GetValue("Health"));
			Assert.AreEqual(2, removedEvents.Count); // exactly: 1 manual + 1 automatic, no spurious extra event
		}
	}
}
#endif
