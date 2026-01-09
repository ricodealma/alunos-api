#!/usr/bin/env dotnet-script
#r "nuget: BCrypt.Net-Next, 4.0.3"

using BCrypt.Net;

if (Args.Count == 0)
{
    Console.WriteLine("Usage: dotnet script generate-hash.csx <password>");
    return;
}

var password = Args[0];
var hash = BCrypt.HashPassword(password, 11);

Console.WriteLine($"Password: {password}");
Console.WriteLine($"BCrypt Hash: {hash}");
