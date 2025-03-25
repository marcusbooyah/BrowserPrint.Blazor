# BrowserPrint.Blazor

[![.NET Test](https://github.com/marcusbooyah/BrowserPrint.Blazor/actions/workflows/test.yml/badge.svg)](https://github.com/marcusbooyah/BrowserPrint.Blazor/actions/workflows/test.yml)
[![NuGet Version](https://img.shields.io/nuget/v/BrowserPrint.Blazor?style=flat&logo=nuget&label=NuGet)](https://www.nuget.org/packages/BrowserPrint.Blazor)

## BrowserPrint.Blazor
Run

```sh
dotnet add package BrowserPrint.Blazor
```


## BrowserPrintProvider

Communication is done via JavaScript interop, so we need a component to handle the calls. Add the `BrowserPrintProvider` component to your page where you want to print.
You can add this to your `App.razor` or `MainLayout.razor` file to make it available to all pages.

```html
@inherits LayoutComponentBase
@using BrowserPrint

<BrowserPrintProvider />

@Body

```

## BrowserPrintService

The `BrowserPrintService` is the service we use to interact with the printer. You can inject this service into your components.

First, call `AddBrowserPrint` in your Startup.
```csharp
builder.Services.AddBrowserPrint();
```

Then inject the service into your component.

```csharp
[Inject]
public BrowserPrintService BrowserPrintService { get; set; }
```

## Printers
To get a list of printers, you can use the `GetAvailablePrintersAsync` method on the `BrowserPrintService`.
```csharp
var printers = await BrowserPrintService.GetAvailablePrintersAsync();
```

## Print

To print a document, you can use the `Print` method on the `BrowserPrintService`.
```csharp
await BrowserPrintService.PrintAsync(printer, "^XA\n^BY2,2,100\n^FO20,20^BC^FD$123^FS\n^XZ");
```