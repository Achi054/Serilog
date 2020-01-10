# Serilog

Integrate Serilog sinks with DotNet Core.

## Introduction

The demo app includes how to setup Serilog logging in ASP Net Core project. Serilog provides diagnostic logging to files, the console, and elsewhere. It is easy to set up, has a clean API, and is portable between recent .NET platforms. Unlike other logging libraries, Serilog is built with powerful structured event data in mind.<br/>
Serilog sinks can be defined through configuration in the `appsettings.json` file.

## Nugets used

- `Serilog.AspNetCore v3.1.1`<br/>
  Packaged with depenencies listed below,
  ```
  Serilog v2.8.0
  Serilog.Extensions.Hosting v3.0.0
  Serilog.Settings.Configuration v3.1.0
  Serilog.Sinks.Console v3.1.1
  Serilog.Sinks.Debug v1.0.1
  Serilog.Sinks.File v4.0.0
  ```

## Changes needed to configure Serilog sinks

- Add below set of Serilog section

  ```json
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Sinks.Debug" ],
    "MinimumLevel": "Error",
    "WriteTo": [
        {
            "Name": "Console",
            "Args": {
                "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
                "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}",
                "formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter, Serilog.Formatting.Compact"
            }
        },
        {
            "Name": "File",
            "Args": {
                "path": "Logs\\serilog-error.txt",
                "rollingInterval": "Day",
                "formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter, Serilog.Formatting.Compact"
            }
        },
        {
            "Name": "Debug",
            "Args": {
                "formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter, Serilog.Formatting.Compact"
            }
        }
    ],
    "Enrich": [ "FromLogContext" ],
    "Properties": {
        "Application": "Serilog Logger"
    }
  }
  ```

- In `Program.cs`, read Serilog configuration from `appsettings.json` and build `ILogger`.

  ```csharp
  public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureAppConfiguration((builderContext, config) =>
                  config.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true))
              .UseSerilog((hostBuilderContext, loggerConfiguration) =>
              {
                  var configuration = hostBuilderContext.Configuration;
                  loggerConfiguration.ReadFrom.Configuration(configuration);
              })
              .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
  ```

## Understanding of Serilog configuration section

### `Using`

Array of Serilog sinks used. In the demo app we are logging to **_Console, File, Debug_**.

```
"Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Sinks.Debug" ]
```

### `MinimumLevel`

Includes the minimum log level. Values include **_Verbose, Debug, Info, Warning, Error, Fatal_**.

```
"MinimumLevel": "Error"
```

### `WriteTo`

Array of logger sinks and configurations, along additional parameters.

```
"WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}",
          "formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter, Serilog.Formatting.Compact"
        }

      },
      {
        "Name": "File",
        "Args": {
          "path": "Logs\\serilog-error.txt",
          "rollingInterval": "Day",
          "formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter, Serilog.Formatting.Compact"
        }
      },
      {
        "Name": "Debug",
        "Args": {
          "formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter, Serilog.Formatting.Compact"
        }
      }
    ]
```

### `Enrich`

Log events can be enriched with properties in various ways. A number of pre-built enrichers are provided as Nugets.

```
"Enrich": [ "FromLogContext" ]
```

### `Properties`

Addition details that go in as part of Serilog log context are defined here.

```
"Properties": {
      "Application": "Serilog Logger"
    }
```

## References

[Serilog.AspNetCore](https://github.com/serilog/serilog-aspnetcore)<br/>
[Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration)<br/>
[Serilog.Sinks.Console](https://github.com/serilog/serilog-sinks-console)<br/>
[Serilog.Sinks.File](https://github.com/serilog/serilog-sinks-file)<br/>
[Serilog.Sinks.Debug](https://github.com/serilog/serilog-sinks-debug)<br/>
[Serilog.Formatting.Compact](https://github.com/serilog/serilog-formatting-compact)
