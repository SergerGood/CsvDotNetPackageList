using CsvDotNetPackageList;
using CsvDotNetPackageList.Configuration;

var settingsLoader = new SettingsLoader();
var settings = settingsLoader.Get<DotNetListSettings>();

var packageListManager = new PackageListManager();
var packages = packageListManager.ProcessAsync(settings);

var csvGenerator = new CsvGenerator();
await csvGenerator.RunAsync(packages);
