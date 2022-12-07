Write-Output "===Day 1==="
Set-Location .\1\
javac .\Program.java && java Program .\input.txt
Set-Location ..

Write-Output "`n===Day 2==="
Set-Location .\2\
javac .\Program.java && java Program .\input.txt
Set-Location ..

Write-Output "`n===Day 3==="
Set-Location .\3\
javac .\Program.java && java Program .\input.txt
Set-Location ..

Write-Output "`n===Day 4==="
dotnet run --project .\4\Program.fsproj

Write-Output "`n===Day 5==="
dotnet run --project .\5\Program.fsproj

Write-Output "`n===Day 6==="
dotnet run --project .\6\Program.csproj

Write-Output "`n===Day 7==="
dotnet run --project .\7\Program.csproj