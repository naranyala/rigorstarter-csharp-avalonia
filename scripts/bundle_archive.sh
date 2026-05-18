#!/bin/bash

set -e

mkdir -p ./build

TIMESTAMP=$(date +"%Y%m%d_%H%M%S")
FOLDER_NAME=$(basename "$PWD")
ARCHIVE_NAME="${FOLDER_NAME}_${TIMESTAMP}.tar.gz"

EXCLUDES=(
  "./bin"
  "./obj"
  "./build"
  "./tests/RigorStarter.Tests/bin"
  "./tests/RigorStarter.Tests/obj"
)

echo "📦 Archiving codebase → ./build/$ARCHIVE_NAME"

# Collect sizes of excluded paths (before tar)
declare -a EXCLUDED_SIZES
for path in "${EXCLUDES[@]}"; do
  # Strip leading ./
  label="${path#./}"
  if [ -e "$path" ] || [ -d "$path" ] || [ -L "$path" ]; then
    size=$(du -sh "$path" 2>/dev/null | cut -f1)
    EXCLUDED_SIZES+=("  (excluded $label — $size)")
  fi
done

tar_args=()
for path in "${EXCLUDES[@]}"; do
  tar_args+=(--exclude="$path")
done
tar_args+=(.)

tar -czf "./build/$ARCHIVE_NAME" "${tar_args[@]}"

FINAL_SIZE=$(du -h "./build/$ARCHIVE_NAME" | cut -f1)
echo "✅ Done — $FINAL_SIZE"
for entry in "${EXCLUDED_SIZES[@]}"; do
  echo "$entry"
done
