REM Get the most recently built package and upload it
FOR /F %%I IN ('DIR pkg\nuget\*.nupkg /b /O:-D') DO set PKG=%%I & GOTO :pushpkg

:pushpkg
echo Pushing package %PKG%
app\Epiphany.SeoMetadata\.nuget\nuget.exe push pkg\nuget\%PKG% -Source https://www.myget.org/F/epiphany/api/v2/package
app\Epiphany.SeoMetadata\.nuget\nuget.exe push pkg\nuget\%PKG%