rm -rf x
dotnet build ./Learns.Pluginable.PluginApp/Learns.Pluginable.PluginApp.csproj
#dotnet publish -c Debug ./Learns.Pluginable.PluginApp/Learns.Pluginable.PluginApp.csproj /p:EnvironmentName=Development
dotnet publish -c Debug ./Learns.Pluginable.WebApp/Learns.Pluginable.WebApp.csproj -p:PublishDir="../x" /p:EnvironmentName=Development
mkdir -p ./x/Plugins/Sample
cp ./Learns.Pluginable.PluginApp/bin/Debug/net6.0/Learns.Pluginable.PluginApp.dll ./x/Plugins/Sample
#cp ./Learns.Pluginable.PluginApp/bin/Debug/net6.0/publish/Learns.Pluginable.PluginApp.dll ./x/Plugins/Sample
#cp -R ./Learns.Pluginable.PluginApp/Areas/Plugins/Sample/Views ./x/Plugins/Sample/Views
cd ./x
dotnet Learns.Pluginable.WebApp.dll