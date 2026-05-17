#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

echo "--- Formatting code with CSharpier ---"
dotnet csharpier format .

echo "--- Cleaning up old builds ---"
dotnet clean rigorstarter-csharp-avalonia.csproj

echo "--- Building the project ---"
dotnet build rigorstarter-csharp-avalonia.csproj --configuration Debug

echo "--- Launching the application ---"
dotnet run --project rigorstarter-csharp-avalonia.csproj --configuration Debug

echo "--- Application closed ---"
