using System;
using UnityEngine;

namespace MiniIT.Utils
{
	public class AdvertisingIdFetcher
	{
		public static void RequestAdvertisingId(Action<string> callback)
		{
			new AdvertisingIdFetcher().DoRequestAdvertisingId(callback);
		}
		
		private Action<string> mCallback;
		
		private void DoRequestAdvertisingId(Action<string> callback)
		{
			mCallback = callback;
			
#if UNITY_EDITOR
			OnAdvertisingIdReceived("0000-0000-0000-0000");
#elif UNITY_ANDROID && UNITY_2020_1_OR_NEWER
			var fetcher = new AndroidJavaObject("com.miniit.android.AdvertisingIdFetcher");
			fetcher.Call("requestAdvertisingId", new AdvertisingIdPluginCallback(OnAdvertisingIdReceived));
#else
			Application.RequestAdvertisingIdentifierAsync(OnAdvertisingIdReceived);
#endif
		}

		private void OnAdvertisingIdReceived(string advertisingId)
		{
			mCallback?.Invoke(advertisingId);
		}

		private void OnAdvertisingIdReceived(string advertisingId, bool trackingEnabled, string errorMsg)
		{
			mCallback?.Invoke(advertisingId);
		}
	}

#if UNITY_ANDROID && UNITY_2020_1_OR_NEWER
	public class AdvertisingIdPluginCallback : AndroidJavaProxy
	{
		private Action<string> mCallback;

		public AdvertisingIdPluginCallback(Action<string> callback) : base("com.miniit.android.AdvertisingIdCallback")
		{
			mCallback = callback;
		}

		public void onResult(string adid)
		{
			mCallback?.Invoke(adid);
		}
	}
#endif
}