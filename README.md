# Introduction 
**MauBlazorHybrid** is a sample application with the goal of deploying to Windows Desktops. This Proof of Concept was concerned with a few key needs:
1. Find a suitable Component Library for the application, which so far seems to be [MudBlazor](https://mudblazor.com/)
2. Support Online/Offline scenarios which is supported via [CommunityToolkit.Datasync](https://github.com/CommunityToolkit/Datasync)
3. Allow easy use of device peripherals like system cameras.  So far, plain old HTML5 and Javascript Interop in Blazor have done the trick.

# Getting Started
The codebase is setup to be pull/run without a lot of manual configuration.  The API project makes use of [localdb](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16) for database needs while the UI uses [SQLite](https://sqlite.org/) for database needs.

# Running the Application
It's advisable to setup a multi-launch profile in your IDE of choice and set the following configuration for each part of the application:
- `MauiBlazorHybrid.Api` set to run `https`
- `MauiBlazorHybrid.UI` set to run as `Windows Machine` (Visual Studio) or `UWP` (Rider)

To verify offline mode, run just `MauiBlazorHybrid.UI` and navigate to `Todo List`.  If you've run the app previously and added Todo items, you should see those entries.  You'll also see a [Snackbar](https://mudblazor.com/components/snackbar#usage) with a message like "Error while refreshing items".  Similarly, if you add/edit a Todo item, you'll see a snackbar for both "Item saved successfully " as well as "error while refreshing items".

To later verify online mode, run both projects, and not "successful sync" snackbar messages for the same operations you saw fail while offline.  In addition, the sync should have pushed the offline records to the API and saved them in the localdb, which you can use [Swagger](https://localhost:7135/swagger/index.html) to do a GET against TodoItems endpoint and see your previously offline-only edits have been applied to the server.

# appsettings.json
The `appsettings.json` file for `MauiBlazorHybrid.UI` is set as an embedded resource which was needed in order for the application to properly access/use the settings.  Because of this quirk, using this in a CI/CD environment would necessitate the pipeline doing a `transform` of the json contents before the application is `compiled/published`.

# Add Support for Android, iOS and MacOS
The `MauiBlazorHybrid.UI.csproj` file has been edited such that only Windows target platform is enabled.  By making edits to line 4, you can target other platforms, but may run into issues specific to each platform, so *caveat emptor*. 