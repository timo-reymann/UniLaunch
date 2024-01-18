SHELL := /bin/bash
.PHONY: help

MACOS_DOTNET_EXECUTABLE_PATH := UniLaunch.MacOS/bin/Release/net7.0/osx-arm64/publish/UniLaunch.MacOS
MACOS_APP_FILE_NAME := UniLaunch.app
MACOS_APP_FILE_EXECUTABLE := UniLaunch
MACOS_APP_FILE_ICON := UniLaunch.MacOS/Resources/Resources/logo-512.png
MACOS_DMG_FILE_NAME := UniLaunchInstaller.dmg
MACOS_DMG_FILE_ICON := UniLaunch.MacOS/InstallerResources/UniLaunch.icns

help: ## Display this help page
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[33m%-30s\033[0m %s\n", $$1, $$2}'

macos-build: macos-build-app macos-build-dmg ## Build for MacOS (needs to be run on a Mac)

macos-build-app: ## Build the MacOS app using dotnet and bundle it to the .app file in dist folder
	@rm -rf dist/$(MACOS_APP_FILE_NAME) || true
	@echo "Build dotnet standalone executable and copy resources to .app file ..." && \
	dotnet publish UniLaunch.MacOS/UniLaunch.MacOS.csproj -c Release
	mkdir -p dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS && \
	cp -r UniLaunch.MacOS/Resources/* dist/$(MACOS_APP_FILE_NAME)/Contents
	@echo "Copy executable to .app file and set its icon using MacOS file system metadata ..." && \
	mkdir -p dist/$(MACOS_APP_FILE_NAME)/Contents && \
	cp $(MACOS_DOTNET_EXECUTABLE_PATH) dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
	DeRez -only icns $(MACOS_APP_FILE_ICON) > dist/tmpicns.rsrc  && \
    Rez -append dist/tmpicns.rsrc -o dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
    SetFile -a C dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
    rm dist/tmpicns.rsrc || true

macos-build-dmg: ## Create the dmg drag and drop installer for the MacOS app and save it in the dist folder
	@rm dist/UniLaunchInstaller.dmg || true
	create-dmg \
      --volname "UniLaunch Installer" \
      --volicon "$(MACOS_DMG_FILE_ICON)" \
      --window-pos 200 120 \
      --window-size 600 400 \
      --icon-size 100 \
      --icon "$(MACOS_APP_FILE_NAME)" 200 190 \
      --hide-extension "$(MACOS_APP_FILE_NAME)" \
      --app-drop-link 400 185 \
      "dist/$(MACOS_DMG_FILE_NAME)" \
      "dist/$(MACOS_APP_FILE_NAME)"
	@find . -type f -name 'rw.*.UniLaunchInstaller.dmg' -exec rm -f {} +

macos-deploy-local: ## Deploy MacOS locally
	sudo cp -r dist/UniLaunch.app /Applications/
