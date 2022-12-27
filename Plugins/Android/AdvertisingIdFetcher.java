
package com.miniit.android;

import com.unity3d.player.UnityPlayer;

import android.content.Context;
import android.os.AsyncTask;
import android.os.Handler;
//import androidx.ads.identifier.AdvertisingIdClient;
//import androidx.ads.identifier.AdvertisingIdInfo;
import com.google.android.gms.ads.identifier.AdvertisingIdClient;
import java.lang.Exception;
import java.lang.Runnable;

public class AdvertisingIdFetcher
{
	private AdvertisingIdCallback callback;
	private Handler handler;
	private TaskCanceler taskCanceler;
	
	public void requestAdvertisingId(AdvertisingIdCallback callback, int timeoutMilliseconds)
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
		
		GetAdIdTask task = new GetAdIdTask(context);
		
		if (timeoutMilliseconds > 0)
		{
			taskCanceler = new TaskCanceler(task);
			
			handler = new Handler();
			handler.postDelayed(taskCanceler, timeoutMilliseconds);
		}

		task.execute();
	}
	
	public void dispose()
	{
		if (taskCanceler != null && handler != null)
		{
			handler.removeCallbacks(taskCanceler);
		}
		handler = null;
		taskCanceler = null;
		callback = null;
	}
	
	private void finish(String adid)
	{
		if (callback != null)
			callback.onResult(adid);
		dispose();
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
			{
				return adInfo.getId();
			}
			
			return "";
		}

		@Override
		protected void onPostExecute(String adid)
		{
			finish(adid);
		}
	}
	
	public class TaskCanceler implements Runnable
	{
		private AsyncTask task;

		public TaskCanceler(AsyncTask task)
		{
			this.task = task;
		}

		@Override
		public void run()
		{
			if (task.getStatus() == AsyncTask.Status.RUNNING)
			{
				task.cancel(true);
				finish("");
			}
		}
	}
}