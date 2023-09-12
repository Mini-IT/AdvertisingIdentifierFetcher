# Advertising Identifier Fetcher

In Unity 2020+ Application.RequestAdvertisingIdentifierAsync returns nothing.
Forum [thread #1](https://forum.unity.com/threads/application-requestadvertisingidentifierasync-removed.972720/)
[thread #2](https://forum.unity.com/threads/application-requestadvertisingidentifierasync-and-unityads.1041748/)

This package uses [AdvertisingIdClient](https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient)

### Usage
```csharp
MiniIT.Utils.AdvertisingIdFetcher.RequestAdvertisingId((advertisingId, trackingEnabled, error) =>
{
	Debug.Log("advertisingId = " + advertisingId);
});
```
Optionally you may specify a timeout in milliseconds for this operation by passing the second parameter. **Default timeout is 1000 ms**. If the operation lasts longer, it is forced to stop and the `callback` will be called with an empty string as a parameter.

If the timeout value is less or equal to zero, no timeout is applied.

### Installation
To add AdvertisingIdFetcher to your project follow these [instructions](https://docs.unity3d.com/Manual/upm-ui-giturl.html) using this Git URL:
```
https://github.com/Mini-IT/AdvertisingIdentifierFetcher.git
```

#### Resolving dependencies
[External Dependency Manager for Unity](https://developers.google.com/unity/archive#external_dependency_manager_for_unity) will help you to resolve dependencies.

Or alternatively you can do it manually following these steps:
- Create custom main gradle template file. Project Settings -> Player -> Android Tab -> Publish Settings -> Custom Main Gradle Template Tick;
- Open Assets/Plugins/Android/mainTemplate.gradle with any text editor and add the following line into `dependencies` section:
```
implementation 'com.google.android.gms:play-services-ads-identifier:16.0.0'
```

### Android only

Advertising Identifier Fetcher uses conditional compilation. On any other platform but Android [Application.RequestAdvertisingIdentifierAsync](https://docs.unity3d.com/ScriptReference/Application.RequestAdvertisingIdentifierAsync.html) will be used.
