# Limbo.Integrations.Skyfish

Integration package with the [Skyfish API](https://api.skyfish.com/).

## Installation

Install via [NuGet](https://www.nuget.org/packages/Limbo.Integrations.Skyfish/):

```
Install-Package Limbo.Integrations.Skyfish -Version 1.0.0-beta004
```

## Getting started

The entry point to accessing the Skyfish API is the `SkyfishHttpService` class. Accessing the API requires a public key, secret key, username and password - if you have those already, you can create a new `SkyfishHttpService` like this:

```csharp
SkyfishHttpService service = SkyfishHttpService.CreateFromKeys("Your public key", "Your secret key", "Your username", "Your password");
```

### Fetching a video

The service instance has a `GetVideo` method that let's you get a video of the authenticated Skyfish account. 

```csharp
@{

    SkyfishVideo video = service.GetVideo(videoId);

}
```

Note that Skyfish doesn't have an iFrame src link when you upload a video, it needs to be generated either via the Skyfish user interface or their API.

Because of that getting a video may take a bit of time as we check if it exists, then if not we request it be generated, and then recheck until it is done.
