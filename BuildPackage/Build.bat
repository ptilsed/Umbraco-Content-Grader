Call nuget.exe restore ..\ContentGrader.sln
Call "C:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe" Package.build.xml /p:Configuration=Release
pause>nul