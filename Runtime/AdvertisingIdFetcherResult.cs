namespace MiniIT.Utils
{
	internal struct AdvertisingIdFetcherResult
	{
		public string advertisingId;
		public bool trackingEnabled;
		public string errorMsg;

		internal AdvertisingIdFetcherResult(string adId = "")
		{
			advertisingId = adId ?? "";
			trackingEnabled = false;
			errorMsg = null;
		}
	}
}