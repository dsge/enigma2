#!/bin/bash
sourceDirectory="$PWD/Library/ScriptAssemblies"
targetDirectory="$PWD/Temp/bin/Debug"

if [ -e "$targetDirectory" ]; then
    echo "$targetDirectory already exists, exiting"
else
    ln -s $sourceDirectory $targetDirectory
    echo "symlink successfully created: $sourceDirectory -> $targetDirectory"
fi
