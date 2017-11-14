$testExitCode = 0

Get-ChildItem src\*.Specifications `
| select -ExpandProperty Name `
| % {
	dotnet test "src\\$_" -c Release --no-build --logger trx --results-directory (Join-Path $(pwd) test_results)
	$testExitCode += $LASTEXITCODE
}

# Get-ChildItem src\*.Integration `
# | select -ExpandProperty Name `
# | % {
# 	dotnet test "src\\$_" -c Release --no-build --logger trx --results-directory (Join-Path $(pwd) test_results)
# 	$testExitCode += $LASTEXITCODE
# }

if ($testExitCode -ne 0) { $host.SetShouldExit($testExitCode); throw; }
