@echo on

rem Build and test with Visual Studio 2015

call "%VS140COMNTOOLS%"vsvars32.bat   
call "%VCINSTALLDIR%"vcvarsall.bat amd64_x86

echo --------- NuGet restore  ---------
nuget restore 

echo --------- Build x86 ---------
msbuild.exe /p:Configuration=Release /t:ReBuild /property:Platform=x86 /v:minimal SimilarImages.sln

echo --------- Build Any CPU ---------
msbuild.exe /p:Configuration=Release /t:ReBuild /property:Platform="Any CPU"  /v:minimal SimilarImages.sln

echo --------- Unit tests ---------
vstest.console.exe "bin\Release\UnitTest.dll" /UseVsixExtensions:false /Logger:trx 
