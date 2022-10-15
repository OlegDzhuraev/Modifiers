using System;

namespace InsaneOne.Modifiers.Example
{
	public static class GameStateLog
	{
		public static event Action<string> MessageReceived;

		public static void Log(string text) => MessageReceived?.Invoke(text);
	}
}