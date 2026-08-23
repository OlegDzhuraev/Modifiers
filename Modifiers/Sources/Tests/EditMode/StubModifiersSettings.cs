using System.Collections.Generic;

namespace InsaneOne.Modifiers.Tests
{
	/// <summary> Minimal IModifiersSettings stub for tests that need to exercise ModifierParam/RuntimeModifier
	/// processing without depending on a real UnityModifiersSettings asset. </summary>
	public class StubModifiersSettings : IModifiersSettings
	{
		readonly Dictionary<string, ModifierParamData> data = new ();

		public void Set(string type, ModifierParamData paramData) => data[type] = paramData;

		public ModifierParamData GetModifierParamData(string paramName) =>
			data.TryGetValue(paramName, out var result) ? result : null;
	}
}
