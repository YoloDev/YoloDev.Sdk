version       := `cat version.txt`
configuration := 'Release'

# List all targets
@list:
  just --list

# Restore packages
restore:
  dotnet restore -p:Configuration={{configuration}}

# Build all projects
build: restore
  dotnet build --configuration {{configuration}} --no-restore

# Pack all projects
pack: build
  dotnet pack --configuration {{configuration}} --no-build

# Build sample projects
build-samples: pack
  #!/usr/bin/env bash
  set -euxo pipefail
  cd sample
  dotnet clean --configuration {{configuration}} -p:YoloDevSdkVersion={{ version }} || true
  rm -rf .packages
  dotnet build --configuration {{configuration}} -p:YoloDevSdkVersion={{ version }}
  dotnet pack --configuration {{configuration}} -p:YoloDevSdkVersion={{ version }} -bl
