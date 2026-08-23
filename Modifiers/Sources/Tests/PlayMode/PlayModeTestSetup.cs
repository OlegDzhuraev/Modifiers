using NUnit.Framework;
using UnityEngine;

namespace InsaneOne.Modifiers.Tests
{
	/// <summary> Runs once for the whole PlayMode test assembly: sets an empty, in-memory
	/// UnityModifiersSettings as active so Modifiable.Awake() doesn't spam "No Modifier Settings was setup"
	/// warnings for every test object, and so GetValue() has deterministic (no-op) processing everywhere. </summary>
	[SetUpFixture]
	public class PlayModeTestSetup
	{
		[OneTimeSetUp]
		public void OneTimeSetUp() => ScriptableObject.CreateInstance<UnityModifiersSettings>().SetAsActive();
	}
}
