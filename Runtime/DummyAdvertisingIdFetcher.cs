using System;

namespace MiniIT.Utils
{
	internal class DummyAdvertisingIdFetcher
	{
		public static void Fetch(Action<string> callback, int timeoutMilliseconds)
		{
			callback?.Invoke("0000-0000-0000-0000");
		}
	}
}