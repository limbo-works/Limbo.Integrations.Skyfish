@echo off
dotnet build src/Limbo.Integrations.Skyfish --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=c:/nuget