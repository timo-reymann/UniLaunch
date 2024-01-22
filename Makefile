# --- BEGIN HELP ---
.PHONY: help
SHELL := /bin/bash

help: require_proper_dev_env ## Display this help page
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[33m%-30s\033[0m %s\n", $$1, $$2}'
# --- END HELP ---

# --- BEGIN GENERAL ---
_is_osx := $(filter Darwin,$(shell uname))
_is_linux = $(filter Linux,$(shell uname))
_is_windows = $(filter Windows_NT,$(OS))

require_osx:
	@if [ -z "$(call _is_osx)" ]; then \
        echo "This task requires OSX platform."; exit 1; \
    fi

require_linux:
	@if [ -z "$(call _is_linux)" ]; then \
        echo "This task requires Linux platform."; exit 1; \
    fi

require_windows:
	@if [ -z "$(call _is_windows)" ]; then \
        echo "This task requires Windows platform."; exit 1; \
    fi

require_proper_dev_env:
	@if [ -z "$(call _is_linux)" ] && [ -z "$(call _is_osx)" ]; then \
        echo "This task requires a proper dev setup. Linux or macOS platform."; exit 1; \
    fi

require_docker:
	@docker --version > /dev/null 2>&1 || (echo "Docker is not installed. Please install Docker to proceed."; exit 1)

clean: ## Clean dist folder and dotnet binary cache
	@rm -rf dist/ || true
	@dotnet clean

_create_dist:
	@mkdir dist/ || true

test: ## Run all tests for the project
	dotnet test


extract_version_command = $(shell awk -F'[<>]' '/<Version>/{print $$3}' Platform.targets)
VERSION := $(extract_version_command)
# --- END GENERAL ---

# --- BEGIN MacOS ---
MACOS_APP_FILE_EXECUTABLE := UniLaunch
MACOS_APP_FILE_ICON := Resources/UniLaunch.icns
MACOS_DMG_FILE_ICON := Resources/UniLaunch.icns

macos-build: macos-build-binary macos-build-app macos-build-dmg ## Build all MacOS targets

macos-build-app: require_osx macos-build-binary ## Build MacOS app file from static file
	$(MAKE) _macos-build-app MACOS_APP_FILE_NAME=UniLaunch-x64.app RID=osx-x64
	$(MAKE) _macos-build-app MACOS_APP_FILE_NAME=UniLaunch-Silicon.app RID=osx-arm64

macos-build-dmg: require_osx macos-build-app ## Create the dmg drag and drop installer for silicon and intel mac
	$(MAKE) _macos-build-dmg MACOS_APP_FILE_NAME=UniLaunch-Silicon.app MACOS_DMG_FILE_NAME=UniLaunchInstaller-Silicon.dmg
	$(MAKE) _macos-build-dmg MACOS_APP_FILE_NAME=UniLaunch-x64.app MACOS_DMG_FILE_NAME=UniLaunchInstaller-x64.dmg

macos-build-binary: require_osx ## Build the binaries for all supported architectures on MacOS
	$(MAKE) _macos-build RID=osx-x64 DOCKER_ARCH=amd64
	$(MAKE) _macos-build RID=osx-arm64 DOCKER_ARCH=linux/arm64/v8

_macos-build: _create_dist
	dotnet publish UniLaunch.MacOS/UniLaunch.MacOS.csproj -r $(RID) -c Release
	cp UniLaunch.MacOS/bin/Release/net8.0/$(RID)/publish/UniLaunch.MacOS ./dist/UniLaunch-$(RID)

_macos-build-app:
	@rm -rf dist/$(MACOS_APP_FILE_NAME) || true
	@echo "Prepare app folder structure ..."
	@mkdir -p dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS && \
	mkdir -p dist/$(MACOS_APP_FILE_NAME)/Contents/Resources
	@echo -en "\
	<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\
	<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">\
	<plist version=\"1.0\">\n\
		<dict>\n\
			<key>CFBundleExecutable</key>\n\
			<string>UniLaunch</string>\n\
			<key>CFBundleIconFile</key>\n\
			<string>$(shell basename $(MACOS_APP_FILE_ICON))</string>\n\
			<key>CFBundleIdentifier</key>\n\
			<string>de.timo_reymann.unilaunch</string>\n\
			<key>CFBundleGetInfoString</key>\n\
			<string>$(VERSION)</string>\n\
			<key>CFBundleShortVersionString</key>\n\
            <string>$(VERSION)</string>\n\
            <key>CFBundleLongVersionString</key>\n\
            <string>$(VERSION)</string>\n\
            <key>CFBundleVersion</key>\n\
           <string>$(VERSION)</string>\n\
		</dict>\n\
	</plist>" > dist/$(MACOS_APP_FILE_NAME)/Contents/Info.plist
	@cp $(MACOS_APP_FILE_ICON) dist/$(MACOS_APP_FILE_NAME)/Contents/Resources
	@echo "Copy executable to .app file and set its icon using MacOS file system metadata ..." && \
	mkdir -p dist/$(MACOS_APP_FILE_NAME)/Contents && \
	cp UniLaunch.MacOS/bin/Release/net8.0/$(RID)/publish/UniLaunch.MacOS dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
	DeRez -only icns $(MACOS_APP_FILE_ICON) > dist/tmpicns.rsrc  && \
    Rez -append dist/tmpicns.rsrc -o dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
    SetFile -a C dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
    rm dist/tmpicns.rsrc || true

_macos-build-dmg:
	@$(eval TMP := $(shell mktemp -d))
	@rm dist/$(MACOS_DMG_FILE_NAME) || true
	cp -r dist/$(MACOS_APP_FILE_NAME) $(TMP)/UniLaunch.app && \
	create-dmg \
      --volname "UniLaunch $(VERSION) Installer" \
      --volicon "$(MACOS_DMG_FILE_ICON)" \
      --window-pos 200 120 \
      --window-size 600 400 \
      --icon-size 100 \
      --icon "UniLaunch.app" 200 190 \
      --hide-extension "UniLaunch.app" \
      --app-drop-link 400 185 \
      --skip-jenkins \
      "dist/$(MACOS_DMG_FILE_NAME)" \
      "$(TMP)/UniLaunch.app"
	@find . -type f -name 'rw.*.UniLaunchInstaller*.dmg' -exec rm -f {} +
# --- END MacOS ---

# --- BEGIN Linux ---
APP_IMAGE_FILE_ICON := Resources/logo-512.png

linux-build: linux-build-binary linux-build-appimage linux-build-deb ## Build all Linux targets

linux-build-binary: ## Build the binaries for all supported architectures on linux
	$(MAKE) _linux-build RID=linux-x64 DOCKER_ARCH=amd64
	$(MAKE) _linux-build RID=linux-arm64 DOCKER_ARCH=arm64

linux-build-appimage: ## Build app image for all supported platforms inside docker
	$(MAKE) _linux-build-appimage ARCH=x64 DOCKER_ARCH=amd64 LINUXDEPLOY_ARCH=x86_64

linux-build-deb: linux-build-binary ## Build deb file for all supported platforms on Linux
	make _linux-deb ARCH=x64 DEB_ARCH=amd64 DOCKER_ARCH=amd64
	make _linux-deb ARCH=arm64 DEB_ARCH=arm64 DOCKER_ARCH=arm64

_linux-build: _create_dist
	dotnet publish UniLaunch.Linux/UniLaunch.Linux.csproj -r $(RID) -c Release
	cp UniLaunch.Linux/bin/Release/net8.0/$(RID)/publish/UniLaunch.Linux ./dist/UniLaunch-$(RID)

_linux-deb: require_docker
	@echo "Set up directory structure ..."
	@rm -rf dist/UniLaunch-$(DEB_ARCH).debsrc || true
	@mkdir -p dist/UniLaunch-$(DEB_ARCH).debsrc/DEBIAN
	@mkdir -p dist/UniLaunch-$(DEB_ARCH).debsrc/usr/local/bin
	@echo -en "\
Package: UniLaunch\n\
Version: $(VERSION)\n\
Maintainer: Timo Reymann\n\
Architecture: $(DEB_ARCH)\n\
Description: UniLaunch\n\
	" >  dist/UniLaunch-$(DEB_ARCH).debsrc/DEBIAN/control
	@echo -en "\
#!/bin/sh\n\
set -e\n\
if [ \"$1\" = \"remove\" ]; then\n\
	CURRENT_USER=${SUDO_USER:-$USER}\n\
 	if [ \"$CURRENT_USER\" = \"root\" ]; then\n\
         exit 0\n\
    fi\n\
     rm -f /home/${CURRENT_USER}/.config/autostart/UniLaunch.desktop > /dev/null 2>&1\n\
fi\n\
	" >  dist/UniLaunch-$(DEB_ARCH).debsrc/DEBIAN/postrm && \
	chmod 755 dist/UniLaunch-$(DEB_ARCH).debsrc/DEBIAN/postrm
	@echo "Copy executable ..."
	@cp ./dist/UniLaunch-linux-$(ARCH) dist/UniLaunch-$(DEB_ARCH).debsrc/usr/local/bin/unilaunch
	@echo "Run deb file build in docker ..."
	docker build -f Dockerfile.DebBuilder.Linux --platform linux/$(DOCKER_ARCH)  . -t unilaunch/deb-builder/linux:$(DOCKER_ARCH)
	docker run --rm -v $(PWD)/dist:/build/dist -it unilaunch/deb-builder/linux:$(DOCKER_ARCH) --build UniLaunch-$(DEB_ARCH).debsrc
	@echo "Move deb file in place"
	@mv dist/UniLaunch-$(DEB_ARCH).debsrc.deb  dist/UniLaunch-$(DEB_ARCH).deb

_linux-build-appimage: require_docker
	@$(eval TMP := $(shell mktemp -d))
	@echo "Prepare docker context ..."
	@mkdir -p $(TMP)/usr/local/bin/
	@cp ./dist/UniLaunch-linux-$(ARCH) $(TMP)/usr/local/bin/unilaunch
	@cp $(APP_IMAGE_FILE_ICON) $(TMP)/.DirIcon
	@echo -en "\
[Desktop Entry]\n\
Name=UniLaunch\n\
Type=Application\n\
Icon=UniLaunch\n\
Categories=Utility\n\
Exec=unilaunch\n\
X-AppImage-Version=$(VERSION)\n\
	" > $(TMP)/UniLaunch.desktop
	echo "Create builder image and build app image using dockerized setup"
	docker build  -f Dockerfile.AppImageBuilder.Linux $(TMP) --build-arg linuxdeploy_arch=$(LINUXDEPLOY_ARCH) --platform linux/$(DOCKER_ARCH) -t unilaunch/appimage-builder/linux:$(DOCKER_ARCH)
	docker run --rm  --platform linux/$(DOCKER_ARCH) -v $(PWD)/dist:/build/dist -it unilaunch/appimage-builder/linux:$(DOCKER_ARCH) \
	  --appimage-extract-and-run \
	  --appdir=../AppDir \
	  --executable=../AppDir/usr/local/bin/unilaunch \
	  --desktop-file=../AppDir/UniLaunch.desktop \
	  --output=appimage \
	  -i ../AppDir/UniLaunch.png
# --- END Linux ---

# -- BEGIN Windows ---
windows-build: windows-build-binary windows-build-installer ## Build all Windows targets

windows-build-binary: ## Build the binaries for all supported architectures on Windows
	$(MAKE) _windows-build RID=win-x64
	$(MAKE) _windows-build RID=win-arm64

windows-build-installer: require_windows ## Build the Windows installer for x64
	@$(eval TMP := $(shell mktemp -d))
	@mkdir -p $(TMP)
	@if [ -z "$(call _is_windows)" ]; then \
            echo "WARNING: This is not required to run on Windows but for ready2run to work it should."; \
    fi

	@cp dist/UniLaunch-win-x64.exe $(TMP)/unilaunch.exe
	@cp Resources/UniLaunch.ico $(TMP)
	@cp LICENSE $(TMP)
	@cp UniLaunch.Windows/Installer/* $(TMP)
	cd $(TMP) && \
	iscc.exe //D"AppVersion=$(VERSION)" //F"UniLaunch-Setup" config.iss
	@cp $(TMP)/dist/UniLaunch-Setup.exe dist/

_windows-build: _create_dist
	dotnet publish UniLaunch.Windows/UniLaunch.Windows.csproj -r $(RID) -c Release
	cp UniLaunch.Windows/bin/Release/net8.0/$(RID)/publish/UniLaunch.Windows.exe dist/UniLaunch-$(RID).exe
# -- END Windows ---
