#!/bin/bash

# Get the absolute path of the current script
SCRIPT_DIR=$(dirname "$(realpath "$0")")

# Define the source paths for the PacketGenerator executable and GenPackets.cs file
SOURCE_DLL="$SCRIPT_DIR/../../Tools/PacketGenerator/bin/PacketGenerator.dll"
SOURCE_CS="$SCRIPT_DIR/../../Tools/PacketGenerator/GenPackets.cs"

# Define the two target directories for copying
TARGET_DIR1="$SCRIPT_DIR/../../Server/Packet"
TARGET_DIR2="$SCRIPT_DIR/../../DummyClient/Packet"

# Ensure the source DLL exists
if [[ ! -f "$SOURCE_DLL" ]]; then
    echo "Source DLL $SOURCE_DLL not found!"
    exit 1
fi

# Run the PacketGenerator executable using dotnet
echo "Running PacketGenerator..."
dotnet "$SOURCE_DLL" "$SCRIPT_DIR/../../Tools/PacketGenerator/PDL.xml"
if [[ $? -ne 0 ]]; then
    echo "Failed to run PacketGenerator."
    exit 1
else
    echo "PacketGenerator ran successfully."
fi

# Ensure GenPackets.cs was created by PacketGenerator
if [[ ! -f "$SOURCE_CS" ]]; then
    echo "GenPackets.cs was not created. Exiting."
    exit 1
fi

# Copy the GenPackets.cs to the first target directory and overwrite if it exists
cp -f "$SOURCE_CS" "$TARGET_DIR1"
if [[ $? -eq 0 ]]; then
    echo "Successfully copied GenPackets.cs to $TARGET_DIR1"
else
    echo "Failed to copy GenPackets.cs to $TARGET_DIR1"
    exit 1
fi

# Copy the GenPackets.cs to the second target directory and overwrite if it exists
cp -f "$SOURCE_CS" "$TARGET_DIR2"
if [[ $? -eq 0 ]]; then
    echo "Successfully copied GenPackets.cs to $TARGET_DIR2"
else
    echo "Failed to copy GenPackets.cs to $TARGET_DIR2"
    exit 1
fi

# Remove the GenPackets.cs file from Tools/PacketGenerator/
rm -f "$SOURCE_CS"
if [[ $? -eq 0 ]]; then
    echo "Successfully removed GenPackets.cs"
else
    echo "Failed to remove GenPackets.cs"
    exit 1
fi
