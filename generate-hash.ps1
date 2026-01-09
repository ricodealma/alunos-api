# Generate BCrypt hash for password
# Usage: .\generate-hash.ps1 "admin123"

param(
    [Parameter(Mandatory=$true)]
    [string]$Password
)

# Install BCrypt.Net-Next if not already installed
$packagePath = Join-Path $env:USERPROFILE ".nuget\packages\bcrypt.net-next\4.0.3"
if (-not (Test-Path $packagePath)) {
    Write-Host "Installing BCrypt.Net-Next..."
    dotnet add package BCrypt.Net-Next --version 4.0.3
}

# Create temporary C# program
$tempCs = @"
using System;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Password required");
            return;
        }
        var hash = BCrypt.Net.BCrypt.HashPassword(args[0], 11);
        Console.WriteLine(hash);
    }
}
"@

$tempDir = New-Item -ItemType Directory -Force -Path ([System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), [System.IO.Path]::GetRandomFileName()))
$tempCsFile = Join-Path $tempDir "HashGen.cs"
$tempCsproj = Join-Path $tempDir "HashGen.csproj"

$csproj = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
  </ItemGroup>
</Project>
"@

Set-Content -Path $tempCsFile -Value $tempCs
Set-Content -Path $tempCsproj -Value $csproj

Push-Location $tempDir
try {
    $result = dotnet run -- $Password
    Write-Host "Password: $Password"
    Write-Host "BCrypt Hash: $result"
    Write-Host ""
    Write-Host "Use this hash in your SQL script:"
    Write-Host $result
} finally {
    Pop-Location
    Remove-Item -Recurse -Force $tempDir
}
