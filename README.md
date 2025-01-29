# CsvDotNetPackageList

## About

CsvDotNetPackageList is a .NET utility designed to generate a CSV file containing a list of all NuGet packages referenced in a .NET solution. This tool is particularly useful who need to audit or document the dependencies used across their projects.

## Features

- Scans a .NET solution to identify all referenced NuGet packages.
- Generates a CSV file listing each package's ID and version.
- Configurable execution via `appsettings.json`.
- Simple and straightforward command-line interface.

## Configuration

### Configuration Options

- Framework: Specifies the target framework to analyze.
- WorkingDirectory: Defines the directory where the solution or project files are located. If left empty, the current directory is used.
- Sources: 

### Example Configuration

To customize the behavior of the tool, you can modify the appsettings.json file as follows:

```json
{
  "DotNetListSettings": {
    "Framework": "net6.0",
    "WorkingDirectory": "C:\\Projects\\MySolution",
    "Sources": [
    ]
  }
}
```
## Usage

After configuring appsettings.json, run the tool.

The tool will generate a packages.csv file in the specified working directory with the following structure:

```csv
Newtonsoft.Json;13.0.1
Serilog;2.10.0
```
