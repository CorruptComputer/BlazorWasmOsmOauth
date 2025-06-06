#!/usr/bin/env bash

curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel STS --install-dir ./dotnet
./dotnet/dotnet --version
./dotnet/dotnet workload install wasm-tools

mkdir publish
./dotnet/dotnet publish BlazorWasmOsmOauth/BlazorWasmOsmOauth.csproj --configuration Release --property PublishDir=/opt/buildhome/repo/publish