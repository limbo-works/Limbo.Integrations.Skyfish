@echo off
dotnet build src/Limbo.Integrations.Skyfish --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget