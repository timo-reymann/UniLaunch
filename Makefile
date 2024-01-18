SHELL := /bin/bash
.PHONY: help

MACOS_APP_FILE_EXECUTABLE := UniLaunch
MACOS_APP_FILE_ICON := UniLaunch.MacOS/Resources/Resources/logo-512.png
MACOS_DMG_FILE_ICON := UniLaunch.MacOS/InstallerResources/UniLaunch.icns

help: ## Display this help page
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[33m%-30s\033[0m %s\n", $$1, $$2}'

macos-build: macos-build-app macos-build-dmg ## Build for MacOS (needs to be run on a Mac)

_macos-build-app: ## Build app for runtime identifier $(RID) and save to $(MACOS_APP_FILE_NAME)
	@rm -rf dist/$(MACOS_APP_FILE_NAME) || true
	@echo "Build dotnet standalone executable and copy resources to .app file ..." && \
	dotnet publish UniLaunch.MacOS/UniLaunch.MacOS.csproj -r $(RID) -c Release
	mkdir -p dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS && \
	cp -r UniLaunch.MacOS/Resources/* dist/$(MACOS_APP_FILE_NAME)/Contents
	@echo "Copy executable to .app file and set its icon using MacOS file system metadata ..." && \
	mkdir -p dist/$(MACOS_APP_FILE_NAME)/Contents && \
	cp UniLaunch.MacOS/bin/Release/net7.0/$(RID)/publish/UniLaunch.MacOS dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
	DeRez -only icns $(MACOS_APP_FILE_ICON) > dist/tmpicns.rsrc  && \
    Rez -append dist/tmpicns.rsrc -o dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
    SetFile -a C dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
    rm dist/tmpicns.rsrc || true

macos-build-app: ## Build MacOS app for silicon and x64
	$(MAKE) _macos-build-app MACOS_APP_FILE_NAME=UniLaunch-x64.app RID=osx-x64
	$(MAKE) _macos-build-app MACOS_APP_FILE_NAME=UniLaunch-Silicon.app RID=osx-arm64

_macos-build-dmg: ## Build dmg installer for $(MACOS_APP_FILE_NAME) resulting in $(MACOS_DMG_FILE_NAME)
	$(eval TMP := $(shell mktemp -d))
	@rm $(MACOS_DMG_FILE_NAME) || true
	cp -r dist/$(MACOS_APP_FILE_NAME) $(TMP)/UniLaunch.app && \
	create-dmg \
      --volname "UniLaunch Installer" \
      --volicon "$(MACOS_DMG_FILE_ICON)" \
      --window-pos 200 120 \
      --window-size 600 400 \
      --icon-size 100 \
      --icon "UniLaunch.app" 200 190 \
      --hide-extension "UniLaunch.app" \
      --app-drop-link 400 185 \
      "dist/$(MACOS_DMG_FILE_NAME)" \
      "$(TMP)/UniLaunch.app"
	@find . -type f -name 'rw.*.UniLaunchInstaller*.dmg' -exec rm -f {} +

macos-build-dmg: ## Create the dmg drag and drop installer for the MacOS app and save it in the dist folder for silicon and x64
	$(MAKE) _macos-build-dmg MACOS_APP_FILE_NAME=UniLaunch-Silicon.app MACOS_DMG_FILE_NAME=UniLaunchInstaller-Silicon.dmg
	$(MAKE) _macos-build-dmg MACOS_APP_FILE_NAME=UniLaunch-x64.app MACOS_DMG_FILE_NAME=UniLaunchInstaller-x64.dmg

_linux-build:
	dotnet publish UniLaunch.Linux/UniLaunch.Linux.csproj -r $(RID) -c Release
	cp UniLaunch.Linux/bin/Release/net7.0/$(RID)/publish/UniLaunch.Linux ./dist/UniLaunch-$(RID)

linux-build:
	$(MAKE) _linux-build RID=linux-x64
	$(MAKE) _linux-build RID=linux-arm64