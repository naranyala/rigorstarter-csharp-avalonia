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
    FILE_SIZE=$(du -h "./build/$ARCHIVE_NAME" | cut -f1)
    echo "Successfully archived to ./build/$ARCHIVE_NAME"
    echo "Final archive size: $FILE_SIZE"
else
    echo "Error occurred during archiving."
    exit 1
fi
