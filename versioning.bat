@echo off

:: Remove the cloned repository to ensure a clean start
rd /s /q GradientSoftware.GithubScripts

:: Get the current branch name
for /f %%i in ('git rev-parse --abbrev-ref HEAD') do set "branch=%%i"
echo Current branch: %branch%

:: Initialize a flag for checking the --script argument
set scriptFilePath=

:: Loop through command line arguments and check if --script is provided
set scriptPath=
for %%a in (%*) do (
    if "%%a"=="--script" (
        set scriptFilePath=true
    )
    if defined scriptFilePath (
        set scriptPath=%%a
        set scriptFilePath=false
    )
)

:: If --script is not provided, clone the repository containing the versioning script
if not defined scriptPath (
    echo Cloning the repository containing the versioning script...
    git clone --single-branch --branch main https://github.com/%GITHUB_USERNAME%/GradientSoftware.GithubScripts.git
    set "scriptPath=./GradientSoftware.GithubScripts/.github/scripts/update-version.csx"
)

:: Check if the script file path is set, if not, exit with an error
if not defined scriptPath (
    echo Error: No script file specified.
    exit /b
)

:: Run the script from the specified or default repository/script path
echo Running script: %scriptPath%
dotnet script %scriptPath% %GITHUB_USERNAME% SCARS.Core %GITHUB_APIKEY% .\SCARS.Core\SCARS.Core.csproj %branch%

:: Remove the cloned repository if it was cloned
if not defined scriptFilePath (
    rd /s /q GradientSoftware.GithubScripts
)
