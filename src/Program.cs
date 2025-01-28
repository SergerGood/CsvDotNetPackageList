using CsvDotNetPackageList;
using CsvDotNetPackageList.Configuration;

var settingsLoader = new SettingsLoader();
var settings = settingsLoader.Get<DotNetListSettings>();

var packageParser = new PackageListManager();
var packages = packageParser.ProcessAsync(settings);

var csvGenerator = new CsvGenerator();
await csvGenerator.RunAsync(packages);
