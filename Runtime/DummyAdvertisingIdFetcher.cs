using UnityEngine;

namespace MiniIT.Utils
{
	internal class DummyAdvertisingIdFetcher
	{
		public static void Fetch(Application.AdvertisingIdentifierCallback callback, int timeoutMilliseconds)
		{
			callback?.Invoke("0000-0000-0000-0000", true, null);
		}
	}
}