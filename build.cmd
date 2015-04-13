@echo off

REM Restore dependencies
call npm install
app\Epiphany.SeoMetadata\.nuget\nuget.exe restore app\Epiphany.SeoMetadata\Epiphany.SeoMetadata.sln

REM Build the nuget package
call grunt nuget