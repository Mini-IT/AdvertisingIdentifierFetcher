# Advertising Identifier Fetcher

In Unity 2020+ Application.RequestAdvertisingIdentifierAsync returns nothing.
Forum [thread #1](https://forum.unity.com/threads/application-requestadvertisingidentifierasync-removed.972720/)
[thread #2](https://forum.unity.com/threads/application-requestadvertisingidentifierasync-and-unityads.1041748/)

This package uses [AdvertisingIdClient](https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient)

### Usage
```csharp
MiniIT.Utils.AdvertisingIdFetcher.RequestAdvertisingId(advertisingId =>
{
	Debug.Log("advertisingId = " + advertisingId);
});
```
