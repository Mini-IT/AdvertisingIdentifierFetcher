using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MiniIT.Utils
{
	internal class UnityAdvertisingIdFetcher
	{
		public static void Fetch(Application.AdvertisingIdentifierCallback callback, int timeoutMilliseconds)
		{
			if (timeoutMilliseconds > 0)
			{
				GetAdvertisingIdAsync(callback, timeoutMilliseconds);
			}
			else
			{
				Application.RequestAdvertisingIdentifierAsync(callback);
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
			
			Application.RequestAdvertisingIdentifierAsync((string advertisingId, bool trackingEnabled, string errorMsg) =>
			{
				result.advertisingId = advertisingId;
				result.trackingEnabled = trackingEnabled;
				result.errorMsg = errorMsg;

				if (!cancellation.IsCancellationRequested)
				{
					cancellation.Cancel();
				}
			});

			if (timeoutMilliseconds > 0)
			{
				try
				{
					await Task.Delay(timeoutMilliseconds, cancellation.Token);
					Debug.Log($"[{nameof(AdvertisingIdFetcher)}] Cancelled by timeout");
				}
				catch (OperationCanceledException)
				{
					// It's ok
					// The request operation finished in time
				}
			}
			
			callback?.Invoke(result.advertisingId, result.trackingEnabled, result.errorMsg);
		}
	}
}