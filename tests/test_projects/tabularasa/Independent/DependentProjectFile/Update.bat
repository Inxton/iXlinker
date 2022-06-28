rem  fill the ts proj name
set "tsProj_file=%CD%\Ts.tsproj"
rem go 5 levels up 
for %%I in ("%CD%\..\..\..\..\..") do set "main=%%~fI"
rem fill the path to the folder where iXlinker.exe is located
set "iXlinkerPath=%main%\src\iXlinker\bin\Debug\net5.0"
set "params=-t %tsProj_file%"
rem run iXlinker
start "iXlinker" /D "%iXlinkerPath%" /WAIT "iXlinker.exe" %params%
rem Clean irrelevant folders and files
rmdir "%CD%\_Boot" /S /Q
rmdir "%CD%\_Libraries" /S /Q
rmdir "%CD%\_CompileInfo" /S /Q
del "%CD%\*.txt" /F /Q
del "%CD%\*.bak" /F /Q
exit
