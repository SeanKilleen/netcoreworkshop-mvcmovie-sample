# NOTE: This is a copy of another repository

I (Sean Killeen) didn't create this work. We can thank Rick Anderson, Scott Hanselman, and Scott Guthrie for putting this sample app together.

I copied this project for use in our sample by referencing <https://github.com/aspnet/AspNetDocs/tree/master/aspnet/mvc/overview/getting-started/introduction/sample/MvcMovie>

The repository in this case was meant as a tutorial for ASP.NET MVC 5. It's a great candidate because 1) it's well-done in the first place and 2) this version has not been ported to .NET Core.

I did make some modifications, adding DI and unit / integration / benchmark tests for later reference.

## Pre-Requisites

Although we'll be moving away from these needs, in order to get started with this workshop you'll need:

* A Windows machine or VM
* Visual Studio
* SQL Server (at least SQL Express with LocalDb). The connection string is configrued for SqlExpress LocalDb but we should be able to modify it pretty quickly if need be.
* The [.NET Portability Analyzer](https://marketplace.visualstudio.com/items?itemName=ConnieYau.NETPortabilityAnalyzer)
* .NET Framework 4.6.1 or later (the sample app begins at v4.6.1)
* .NET Core v3.x
* An internet connection

You also may wish to install:

* Docker for Windows (in case we want to explore containerization)

After we get past the initial conversion phase, things will get much more flexible.'

## Helpful Links / References

* [MS Guidance on Migrating from ASP.NET to .NET Core 3.0](https://docs.microsoft.com/en-us/aspnet/core/migration/proper-to-2x/?view=aspnetcore-3.0)
* [The MVC Specific page for the above guidance](https://docs.microsoft.com/en-us/aspnet/core/migration/mvc?view=aspnetcore-3.0)
* [The Little ASP.NET Core Book](https://recaffeinate.co/book/)

## Choose Your Own Adventure

Once we've done the core of the updates, we'll have some places we can go. Suggestions:

* Containerize the app using Docker
* Add .NET Core Health Checks
* Re-implement -- or even better, Migrate -- the identity & authentication. 
* Add an Azure DevOps build with a YAML file in the repo.
