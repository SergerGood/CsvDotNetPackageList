dotnet publish -c Release -r win-x64 \
 -p:PublishSingleFile=true --self-contained true \
 -p:DebugType=None -p:DebugSymbols=false -p:WarningLevel=0 -v minimal \
 -p:AssemblyName=csv-package-list -o out
