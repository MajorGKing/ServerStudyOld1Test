#!/bin/bash

# Get the absolute path of the current script
SCRIPT_DIR=$(dirname "$(realpath "$0")")

# Define the source paths for the PacketGenerator executable and the three files
SOURCE_DLL="$SCRIPT_DIR/../../Tools/PacketGenerator/bin/PacketGenerator.dll"
SOURCE_GENPACKETS_CS="$SCRIPT_DIR/../../Tools/PacketGenerator/bin/GenPackets.cs"
SOURCE_SERVER_PACKETMANAGER_CS="$SCRIPT_DIR/../../Tools/PacketGenerator/bin/ServerPacketManager.cs"
SOURCE_CLIENT_PACKETMANAGER_CS="$SCRIPT_DIR/../../Tools/PacketGenerator/bin/ClientPacketManager.cs"

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
if [[ ! -f "$SOURCE_GENPACKETS_CS" ]]; then
    echo "GenPackets.cs was not created. Exiting."
    exit 1
fi

# Ensure ServerPacketManager.cs was created by PacketGenerator
if [[ ! -f "$SOURCE_SERVER_PACKETMANAGER_CS" ]]; then
    echo "ServerPacketManager.cs was not created. Exiting."
    exit 1
fi

# Ensure ClientPacketManager.cs was created by PacketGenerator
if [[ ! -f "$SOURCE_CLIENT_PACKETMANAGER_CS" ]]; then
    echo "ClientPacketManager.cs was not created. Exiting."
    exit 1
fi

# Copy GenPackets.cs to the first target directory (Server/Packet) and overwrite if it exists
cp -f "$SOURCE_GENPACKETS_CS" "$TARGET_DIR1"
if [[ $? -eq 0 ]]; then
    echo "Successfully copied GenPackets.cs to $TARGET_DIR1"
else
    echo "Failed to copy GenPackets.cs to $TARGET_DIR1"
    exit 1
fi

# Copy GenPackets.cs to the second target directory (DummyClient/Packet) and overwrite if it exists
cp -f "$SOURCE_GENPACKETS_CS" "$TARGET_DIR2"
if [[ $? -eq 0 ]]; then
    echo "Successfully copied GenPackets.cs to $TARGET_DIR2"
else
    echo "Failed to copy GenPackets.cs to $TARGET_DIR2"
    exit 1
fi

# Copy ServerPacketManager.cs to the first target directory (Server/Packet) and overwrite if it exists
cp -f "$SOURCE_SERVER_PACKETMANAGER_CS" "$TARGET_DIR1"
if [[ $? -eq 0 ]]; then
    echo "Successfully copied ServerPacketManager.cs to $TARGET_DIR1"
else
    echo "Failed to copy ServerPacketManager.cs to $TARGET_DIR1"
    exit 1
fi

# Copy ClientPacketManager.cs to the second target directory (DummyClient/Packet) and overwrite if it exists
cp -f "$SOURCE_CLIENT_PACKETMANAGER_CS" "$TARGET_DIR2"
if [[ $? -eq 0 ]]; then
    echo "Successfully copied ClientPacketManager.cs to $TARGET_DIR2"
else
    echo "Failed to copy ClientPacketManager.cs to $TARGET_DIR2"
    exit 1
fi

# Remove GenPackets.cs from Tools/PacketGenerator/bin/
rm -f "$SOURCE_GENPACKETS_CS"
if [[ $? -eq 0 ]]; then
    echo "Successfully removed GenPackets.cs"
else
    echo "Failed to remove GenPackets.cs"
    exit 1
fi

# Remove ServerPacketManager.cs from Tools/PacketGenerator/bin/
rm -f "$SOURCE_SERVER_PACKETMANAGER_CS"
if [[ $? -eq 0 ]]; then
    echo "Successfully removed ServerPacketManager.cs"
else
    echo "Failed to remove ServerPacketManager.cs"
    exit 1
fi

# Remove ClientPacketManager.cs from Tools/PacketGenerator/bin/
rm -f "$SOURCE_CLIENT_PACKETMANAGER_CS"
if [[ $? -eq 0 ]]; then
    echo "Successfully removed ClientPacketManager.cs"
else
    echo "Failed to remove ClientPacketManager.cs"
    exit 1
fi
