# .NET 8 Windows Forms migration

Both the calendar library and the demo target `net8.0-windows` using SDK-style
projects. The library can be referenced by .NET 8 Windows Forms applications,
including MichaeloCRM.

## Build and run

Use Windows with the .NET 8 SDK or a newer SDK supporting this target.

```powershell
dotnet restore CalendarNet.sln
dotnet build CalendarNet.sln --configuration Release
dotnet run --project CalendarNetDemo --configuration Release
dotnet run --project tests/CalendarNet.SmokeTests --configuration Release
```

Framework-dependent demo deployments require the .NET Desktop Runtime 8.
These projects no longer target .NET Framework 3.5.

## Project changes

- Keep existing namespaces, the public calendar API and assembly metadata.
- Embed calendar icons under their original resource names.
- Preserve WinForms designer nesting for the calendar, demo form and resources.
- Initialize the demo through `ApplicationConfiguration.Initialize` with
  SystemAware DPI handling.
- Exclude the demo's empty, unused .NET Framework settings wrapper from
  compilation. Its source is retained; the demo does not read or write settings.
- Declare the Windows-only API contract explicitly while retaining the original
  assembly metadata.
- Validate Debug and Release builds and a local rendering/resource smoke test
  through the Windows GitHub Actions workflow.

The smoke test checks embedded icons, all months, day-view rendering, themes,
resizing and demo initialization. It creates screenshots in its ignored build
output directory. Local validation passed; GitHub Actions results are separate
and must be checked after publishing the branch.

## Scope

This branch changes framework and build setup. It does not integrate the
control into MichaeloCRM, add database persistence, localize the calendar,
change recurrence rules or change holiday definitions.

Reference: [Microsoft desktop SDK properties](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props-desktop).
