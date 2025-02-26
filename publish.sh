dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:DebugType=None -p:DebugSymbols=false --self-contained true -p:AssemblyName=csv-package-list --property:OutputPath=out
