dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:DebugType=None -p:DebugSymbols=false -p:WarningLevel=0 -v minimal --self-contained true -p:AssemblyName=csv-package-list -o out
