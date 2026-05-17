#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

echo "--- Cleaning up old builds ---"
dotnet clean

echo "--- Building the project ---"
dotnet build --configuration Debug

echo "--- Launching the application ---"
dotnet run --configuration Debug

echo "--- Application closed ---"
