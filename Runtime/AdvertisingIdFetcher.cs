using System;
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
		public static void RequestAdvertisingId(Action<string> callback, int timeoutMilliseconds = 1000)
		{
			new AdvertisingIdFetcher().DoRequestAdvertisingId(callback, timeoutMilliseconds);
		}
		
		private Action<string> _callback;
		
		private void DoRequestAdvertisingId(Action<string> callback, int timeoutMilliseconds = 0)
		{
			_callback = callback;
			
#if UNITY_EDITOR
			OnAdvertisingIdReceived("0000-0000-0000-0000");
#elif UNITY_ANDROID && UNITY_2020_1_OR_NEWER && !AMAZON_STORE
			var fetcher = new AndroidJavaObject("com.miniit.android.AdvertisingIdFetcher");
			fetcher.Call("requestAdvertisingId", new AdvertisingIdPluginCallback(OnAdvertisingIdReceived), timeoutMilliseconds);
#else
			Application.RequestAdvertisingIdentifierAsync(OnAdvertisingIdReceived);
#endif
		}

		private void OnAdvertisingIdReceived(string advertisingId)
		{
			_callback?.Invoke(advertisingId);
		}

		private void OnAdvertisingIdReceived(string advertisingId, bool trackingEnabled, string errorMsg)
		{
			_callback?.Invoke(advertisingId);
		}
	}

#if UNITY_ANDROID && UNITY_2020_1_OR_NEWER && !AMAZON_STORE
	public class AdvertisingIdPluginCallback : AndroidJavaProxy
	{
		private Action<string> _callback;

		public AdvertisingIdPluginCallback(Action<string> callback) : base("com.miniit.android.AdvertisingIdCallback")
		{
			_callback = callback;
		}

		public void onResult(string adid)
		{
			_callback?.Invoke(adid);
		}
	}
#endif
}