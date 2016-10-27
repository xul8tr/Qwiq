ECHO OFF

:: Import VS tools
IF DEFINED VS140COMNTOOLS (
  CALL "%VS140COMNTOOLS%vsvars32.bat"
) ELSE (
  IF DEFINED VS120COMNTOOLS (
    CALL "%VS120COMNTOOLS%vsvars32.bat"
  ) ELSE (
    IF DEFINED VS110COMNTOOLS (
      CALL "%VS110COMNTOOLS%vsvars32.bat"
    ) ELSE (
      EXIT /B 1
    )
  )
)

:: Init workspace
IF EXIST init.cmd (
  CALL init.cmd
)

:: Install packages we need
nuget restore .\Qwiq.sln -NonInteractive -DisableParallelProcessing

:: Build
msbuild .\Qwiq.sln /nologo /clp:"NoSummary;Verbosity=minimal" /m /p:Configuration=Release /t:Rebuild /nodeReuse:false
IF ERRORLEVEL 1 (
  EXIT /B %ERRORLEVEL%
)

:: Build for all target frameworks
powershell -noprofile -executionpolicy bypass -command "$v = @('4.6','4.6.1','4.6.2'); gci -Filter *.csproj -Recurse -Exclude *.Tests.csproj | %% { foreach ($version in $v) { $lib = ($version.replace('.', '')); & msbuild $_ /nologo /clp:Verbosity=minimal /m /nodeReuse:false /p:Configuration=Release /p:TargetFrameworkVersion=$version /p:OutputPath=bin\Release\net$lib\ } }"
IF ERRORLEVEL 1 (
  EXIT /B %ERRORLEVEL%
)

:: Package up our .csproj items into packages
powershell -noprofile -executionpolicy bypass -command "gci -Filter *.csproj -Recurse -Exclude *.Tests.csproj | %% { nuget pack $_.FullName -IncludeReferencedProjects -Symbols -Properties Configuration=Release }"
