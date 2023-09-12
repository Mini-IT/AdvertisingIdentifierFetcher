using UnityEngine;

namespace MiniIT.Utils
{
	public class AdvertisingIdFetcher
	{
		/// <summary>
		/// Asyncronously requests an advertising id.
		/// </summary>
		/// <param name="callback">Will be called when the request is finished. The parameter will contain an advertising id
		/// or an empty string if the request is failed or interrupted by timeout</param>
		/// <param name="timeoutMilliseconds">Value <= 0 means no timeout. If the operation lasts more than this much milliseconds,
		/// it is forced to stop and the <c>callback</c> will be called with an empty string as a parameter</param>
		public static void RequestAdvertisingId(Application.AdvertisingIdentifierCallback callback, int timeoutMilliseconds = 1000)
		{
#if UNITY_EDITOR
			DummyAdvertisingIdFetcher.Fetch(callback, timeoutMilliseconds);
#elif UNITY_ANDROID && UNITY_2020_1_OR_NEWER
			AndroidAdvertisingIdFetcher.Fetch(callback, timeoutMilliseconds);
#else
			UnityAdvertisingIdFetcher.Fetch(callback, timeoutMilliseconds);
#endif
		}
	}
}