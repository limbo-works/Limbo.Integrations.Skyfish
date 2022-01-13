@echo off
dotnet build src/Limbo.Integrations.Skyfish --configuration Release /t:rebuild /t:pack -p:BuildTools=1 -p:PackageOutputPath=../../releases/nuget