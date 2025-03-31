@echo off

REM Step 1: Clean old test results
for /d %%d in (*Tests) do (
    if exist "%%d\TestResults" (
        echo Deleting old test results from %%d...
        rmdir /s /q "%%d\TestResults"
    )
)

REM Step 2: Build the solution
dotnet build

REM Step 3: Run all test projects with coverage collection
dotnet test SCARS-Core.Tests --collect:"XPlat Code Coverage"

REM Step 4: Generate combined coverage report (only fresh results)
reportgenerator -reports:"**/TestResults/**/coverage.cobertura.xml" -targetdir:"test-report" -reporttypes:Html

echo.
echo ✅ Test report generated at test-report\index.html