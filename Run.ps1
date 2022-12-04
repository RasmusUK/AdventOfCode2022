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