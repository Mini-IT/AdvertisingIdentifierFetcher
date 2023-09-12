using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MiniIT.Utils
{
	internal class AndroidAdvertisingIdFetcher
	{
		public static void Fetch(Application.AdvertisingIdentifierCallback callback, int timeoutMilliseconds)
		{
			if (timeoutMilliseconds > 0)
			{
				GetAdvertisingIdAsync(callback, timeoutMilliseconds);
			}
			else
			{
				var result = GetAndroidAdvertisingId();
				callback?.Invoke(result.advertisingId, result.trackingEnabled, result.errorMsg);
			}
		}

		private static async void GetAdvertisingIdAsync(Application.AdvertisingIdentifierCallback callback, int timeoutMilliseconds)
		{
			var result = new AdvertisingIdFetcherResult();

			var cancellation = new CancellationTokenSource();
			if (timeoutMilliseconds > 0)
			{
				cancellation.CancelAfter(timeoutMilliseconds);
			}

			try
			{
				result = await Task.Run(GetAndroidAdvertisingId, cancellation.Token);
			}
			catch (OperationCanceledException)
			{
				result.errorMsg = "Cancelled by timeout";
				Debug.Log($"[{nameof(AdvertisingIdFetcher)}] Cancelled by timeout");
			}

			callback?.Invoke(result.advertisingId, result.trackingEnabled, result.errorMsg);
		}

		private static AdvertisingIdFetcherResult GetAndroidAdvertisingId()
		{
			var result = new AdvertisingIdFetcherResult();

			try
			{
				if (AndroidJNI.AttachCurrentThread() == 0)
				{
					AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaClass adIdClientClass = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
					AndroidJavaObject adIdInfo = adIdClientClass.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);

					result.advertisingId = adIdInfo.Call<string>("getId").ToString();
					result.trackingEnabled = adIdInfo.Call<bool>("isLimitAdTrackingEnabled");
				}
			}
			catch (Exception e)
			{
				result.errorMsg = e.Message;
				Debug.Log($"[{nameof(AdvertisingIdFetcher)}] Failed to get android advertising ID: {e}");
			}

			return result;
		}
	}
}