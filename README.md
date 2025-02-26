## About

CsvDotNetPackageList is a .NET utility designed to generate a CSV file containing a list of all NuGet packages referenced in a .NET solution including transitive dependencies. This tool is particularly useful who need to audit or document the dependencies used across their projects.

## Features

- Scans a .NET solution to identify all referenced NuGet packages.
- Generates a CSV file listing each package's ID and version.
- Configurable execution via `appsettings.json`.
- Simple and straightforward command-line interface.

## Configuration

### Configuration Options

- **Framework** (_optional_): If not specified, the analysis will search for packages across all target frameworks.
- **WorkingDirectory** (_optional_): The base directory used for resolving relative paths and generating the final CSV output file.
- **Sources** (_required_): A list of project or solution files to analyze. If absolute file paths are provided, WorkingDirectory will only be used when generating the final CSV report.

### Example Configuration

To customize the behavior of the tool, you can modify the appsettings.json file as follows:

```json
{
  "DotNetListSettings": {
    "Framework": "net8.0",
    "Sources": [
      "C:\\Projects\\MySolution.sln"
    ]
  }
}
```

```json
{
  "DotNetListSettings": {
    "Framework": "net8.0",
    "WorkingDirectory": "C:\\Projects\\MySolution",
    "Sources": [
      "Project1.csproj",
      "Project2.csproj"
    ],
  }
}
```

## Usage

Follow these steps to use the application:

1. Clone the repository:
```bash
git clone https://github.com/SergerGood/CsvDotNetPackageList.git
```
2. Publish the application. The result will be in subfolder _out_.
```bash
./publish.sh
```
3. Configure the application by modifying _appsettings.json_ according to your needs.
4. Run the tool using the generated _csv-package-list_ executable and analyze the dependencies.
5. The tool will generate a _packages.csv_ file in the specified working directory with the following structure:
```csv
Newtonsoft.Json;13.0.1
Serilog;2.10.0
```
