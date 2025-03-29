# AufseherServer
This is an ASP.NET API Server written in C# and .NET 9.

It aims to securely grant access to third-party instances for data collected by the private Discord bot "Der Aufseher" (German for: "The Observer").
The Aufseher Discord app collects user data by observing their activity on a Discord server. This data is stored in MongoDB clusters.

This is more of a fun project and, at this time, does not aim to achieve any revenue or consistent use cases.

# Contributing
Feel free to contribute by opening an issue or pull request. 

# Running this on your own
You can run this API server on your own. Clone the repo, then head to the `Settings.cs` file; you'll see all the properties the server relies on. You then can create an `appsettings.Development.json` file containing your configuration. 
In `Startup.cs` you can specify the IP & ports used.

Note: This server will not run on its own, you'll need some sort of basic ASP.NET project created where you can merge this one into. Some default files required to run the Server are on `.gitignore` as I couldn't see any benefit in including them here. 
You'll also need to set up a proper MongoDB cluster. I recommend Atlas. Required database models/DTOs can be found in the `AufseherServer.Models` namespace.
