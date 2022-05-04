rem  fill the ts proj name
set "tsProj_file=%CD%\Ts\Ts.tsproj"
rem go 4 levels up 
for %%I in ("%CD%\..\..\..\..") do set "main=%%~fI"
rem fill the path to the folder where iXlinker.exe is located
set "iXlinkerPath=%main%\src\iXlinker\bin\Debug\net5.0"
set "params=-t %tsProj_file% -n 25"
rem run iXlinker
start "iXlinker" /D "%iXlinkerPath%" /WAIT "iXlinker.exe" %params%
rmdir "%CD%\Ts\PLC\_Libraries\beckhoff automation gmbh" /S /Q