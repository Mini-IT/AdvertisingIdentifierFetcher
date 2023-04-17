using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MiniIT.Utils
{
	internal class AndroidAdvertisingIdFetcher
	{
		public static void Fetch(Action<string> callback, int timeoutMilliseconds)
		{
			if (timeoutMilliseconds > 0)
			{
				GetAdvertisingIdAsync(callback, timeoutMilliseconds);
			}
			else
			{
				string adId = GetAndroidAdvertisingId();
				callback?.Invoke(adId);
			}
		}

		private static async void GetAdvertisingIdAsync(Action<string> callback, int timeoutMilliseconds)
		{
			string result = "";
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
				Debug.Log($"[{nameof(AdvertisingIdFetcher)}] Cancelled by timeout");
			}

			callback?.Invoke(result);
		}

		public static string GetAndroidAdvertisingId()
		{
			string advertisingID = "";
			try
			{
				AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaClass adIdClientClass = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
				AndroidJavaObject adIdInfo = adIdClientClass.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);

				advertisingID = adIdInfo.Call<string>("getId").ToString();
			}
			catch (Exception e)
			{
				Debug.Log($"[{nameof(AdvertisingIdFetcher)}] Failed to get android advertising ID: {e}");
			}
			return advertisingID;
		}
	}
}