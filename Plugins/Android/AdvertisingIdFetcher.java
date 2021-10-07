
package com.miniit.android;

import com.unity3d.player.UnityPlayer;

import android.content.Context;
import android.os.AsyncTask;
//import androidx.ads.identifier.AdvertisingIdClient;
//import androidx.ads.identifier.AdvertisingIdInfo;
import com.google.android.gms.ads.identifier.AdvertisingIdClient;
import java.lang.Exception;

public class AdvertisingIdFetcher
{
	private AdvertisingIdCallback callback;
	
	public void requestAdvertisingId(AdvertisingIdCallback callback)
	{
		if (callback == null)
			return;
		
		Context context = UnityPlayer.currentActivity.getApplicationContext();
		
		// if (!AdvertisingIdClient.isAdvertisingIdProviderAvailable(context))
		// {
			// callback.onResult("");
			// return;
		// }

		this.callback = callback;
		
		new GetAdIdTask(context).execute();
	}
	
	private class GetAdIdTask extends AsyncTask<String, Integer, String>
	{
		private Context context;
		
		public GetAdIdTask(Context context)
		{
			super();
			this.context = context;
		}
		
		@Override
		protected String doInBackground(String... strings)
		{
			// if (!AdvertisingIdClient.isAdvertisingIdProviderAvailable(context))
				// return "";
			
			AdvertisingIdClient.Info adInfo;
			adInfo = null;
			try
			{
				adInfo = AdvertisingIdClient.getAdvertisingIdInfo(context);
				if (adInfo.isLimitAdTrackingEnabled()) // check if user has opted out of tracking
				{
					return "";
				}
			}
			catch (Exception e)
			{
				e.printStackTrace();
			}
			
			if (adInfo != null)
				return adInfo.getId();
			
			return "";
		}

		@Override
		protected void onPostExecute(String adid)
		{
			if (callback != null)
				callback.onResult(adid);
		}
	}
}