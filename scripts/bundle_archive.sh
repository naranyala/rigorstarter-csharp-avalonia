#!/bin/bash

# Create build directory if it doesn't exist
mkdir -p ./build

# Generate precise timestamp (YYYYMMDD_HHMMSS)
TIMESTAMP=$(date +"%Y%m%d_%H%M%S")
FOLDER_NAME=$(basename "$PWD")
ARCHIVE_NAME="${FOLDER_NAME}_${TIMESTAMP}.tar.gz"

echo "Archiving codebase to ./build/$ARCHIVE_NAME..."

# Create the archive
# Exclude bin and obj directories to remove compilation artifacts
# Also exclude the build directory itself to avoid recursive archiving
tar -czf "./build/$ARCHIVE_NAME" \
    --exclude='./bin' \
    --exclude='./obj' \
    --exclude='./build' \
    --exclude='./tests/RigorStarter.Tests/bin' \
    --exclude='./tests/RigorStarter.Tests/obj' \
    .

if [ $? -eq 0 ]; then
    echo "Successfully archived to ./build/$ARCHIVE_NAME"
    ls -lh "./build/$ARCHIVE_NAME"
else
    echo "Error occurred during archiving."
    exit 1
fi
