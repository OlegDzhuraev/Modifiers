using System;

namespace Example.Sources
{
	public static class GameStateLog
	{
		public static event Action<string> MessageReceived;

		public static void Log(string text)
		{
			MessageReceived?.Invoke(text);
		}
	}
}