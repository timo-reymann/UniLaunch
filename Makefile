SHELL := /bin/bash
.PHONY: help

MACOS_APP_FILE_EXECUTABLE := UniLaunch
MACOS_APP_FILE_ICON := UniLaunch.MacOS/Resources/Resources/logo-512.png
MACOS_DMG_FILE_ICON := UniLaunch.MacOS/InstallerResources/UniLaunch.icns

APP_IMAGE_FILE_ICON := UniLaunch.Linux/Resources/icon.png

help: ## Display this help page
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[33m%-30s\033[0m %s\n", $$1, $$2}'

clean: ## Clean dist folder and dotnet binary cache
	rm -rf dist/ || true
	dotnet clean
	mkdir dist/

macos-build: macos-build-binary macos-build-app macos-build-dmg ## Build all MacOS targets

macos-build-app: ## Build MacOS app file from static file
	$(MAKE) _macos-build-app MACOS_APP_FILE_NAME=UniLaunch-x64.app RID=osx-x64
	$(MAKE) _macos-build-app MACOS_APP_FILE_NAME=UniLaunch-Silicon.app RID=osx-arm64

macos-build-dmg: ## Create the dmg drag and drop installer
	$(MAKE) _macos-build-dmg MACOS_APP_FILE_NAME=UniLaunch-Silicon.app MACOS_DMG_FILE_NAME=UniLaunchInstaller-Silicon.dmg
	$(MAKE) _macos-build-dmg MACOS_APP_FILE_NAME=UniLaunch-x64.app MACOS_DMG_FILE_NAME=UniLaunchInstaller-x64.dmg

macos-build-binary: ## Build the binary for MacOS
	$(MAKE) _linux-build RID=osx-x64 DOCKER_ARCH=amd64
	$(MAKE) _linux-build RID=osx-arm64 DOCKER_ARCH=arm64

_macos-build:
	dotnet publish UniLaunch.MacOS/MacOS.Linux.csproj -r $(RID) -c Release
	cp UniLaunch.MacOS/bin/Release/net7.0/$(RID)/publish/UniLaunch.MacOS ./dist/UniLaunch-$(RID)

_macos-build-app:
	@rm -rf dist/$(MACOS_APP_FILE_NAME) || true
	mkdir -p dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS && \
	cp -r UniLaunch.MacOS/Resources/* dist/$(MACOS_APP_FILE_NAME)/Contents
	@echo "Copy executable to .app file and set its icon using MacOS file system metadata ..." && \
	mkdir -p dist/$(MACOS_APP_FILE_NAME)/Contents && \
	cp UniLaunch.MacOS/bin/Release/net7.0/$(RID)/publish/UniLaunch.MacOS dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
	DeRez -only icns $(MACOS_APP_FILE_ICON) > dist/tmpicns.rsrc  && \
    Rez -append dist/tmpicns.rsrc -o dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
    SetFile -a C dist/$(MACOS_APP_FILE_NAME)/Contents/MacOS/$(MACOS_APP_FILE_EXECUTABLE) && \
    rm dist/tmpicns.rsrc || true

_macos-build-dmg:
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

linux-build: linux-build-binary linux-build-appimage linux-build-deb ## Build all linux targets

linux-build-binary: ## Build the binary for linux
	$(MAKE) _linux-build RID=linux-x64 DOCKER_ARCH=amd64
	$(MAKE) _linux-build RID=linux-arm64 DOCKER_ARCH=arm64

linux-build-appimage: ## Build appfiles
	$(MAKE) _linux-build-appimage ARCH=x64 DOCKER_ARCH=amd64 LINUXDEPLOY_ARCH=static-x86_64
	$(MAKE) _linux-build-appimage ARCH=arm64 DOCKER_ARCH=arm64 LINUXDEPLOY_ARCH=aarch64

linux-build-deb: ## Build deb files
	make _linux-deb ARCH=x64 DEB_ARCH=amd64 DOCKER_ARCH=amd64
	make _linux-deb ARCH=arm64 DEB_ARCH=arm64 DOCKER_ARCH=arm64

_linux-build:
	dotnet publish UniLaunch.Linux/UniLaunch.Linux.csproj -r $(RID) -c Release
	cp UniLaunch.Linux/bin/Release/net7.0/$(RID)/publish/UniLaunch.Linux ./dist/UniLaunch-$(RID)

_linux-deb:
	@echo "Set up directory structure ..."
	@mkdir -p dist/UniLaunch-$(DEB_ARCH).debsrc/DEBIAN
	@mkdir -p dist/UniLaunch-$(DEB_ARCH).debsrc/usr/local/bin
	@echo -e "\
	Package: UniLaunch\n\
	Version: 0.0.1\n\
	Maintainer: Timo Reymann\n\
	Architecture: $(DEB_ARCH)\n\
	Description: UniLaunch\
	" >  dist/UniLaunch-$(DEB_ARCH).debsrc/DEBIAN/control
	@echo "Copy executable ..."
	@cp ./dist/UniLaunch-linux-$(ARCH) dist/UniLaunch-$(DEB_ARCH).debsrc/usr/local/bin/unilaunch
	echo "Run deb file build in docker ..."
	docker build -f Dockerfile.DebBuilder.Linux --platform linux/$(DOCKER_ARCH)  . -t unilaunch/deb-builder/linux:$(DOCKER_ARCH)
	docker run --rm -v $(PWD)/dist:/build/dist -it unilaunch/deb-builder/linux:$(DOCKER_ARCH) --build UniLaunch-$(DEB_ARCH).debsrc
	@echo "Move deb file in place"
	@rm -rf dist/UniLaunch-$(DEB_ARCH).debsrc
	@mv dist/UniLaunch-$(DEB_ARCH).debsrc.deb  dist/UniLaunch-$(DEB_ARCH).deb

_linux-build-appimage:
	$(eval TMP := $(shell mktemp -d))
	@echo "Prepare docker context ..."
	@mkdir -p $(TMP)/usr/local/bin/
	@cp ./dist/UniLaunch-linux-$(ARCH) $(TMP)/usr/local/bin/unilaunch
	@cp $(APP_IMAGE_FILE_ICON) $(TMP)/.DirIcon
	@cp -r UniLaunch.Linux/AppImageResources/* $(TMP)
	echo "Create builder image and build app image using dockerized setup"
	docker build  -f Dockerfile.AppImageBuilder.Linux $(TMP) --build-arg linuxdeploy_arch=$(LINUXDEPLOY_ARCH) --platform linux/$(DOCKER_ARCH) -t unilaunch/appimage-builder/linux:$(DOCKER_ARCH)
	docker run --rm  --platform linux/$(DOCKER_ARCH) -v $(PWD)/dist:/build/dist -it unilaunch/appimage-builder/linux:$(DOCKER_ARCH) \
		--appimage-extract-and-run \
		--appdir=../AppDir \
		--executable=../AppDir/usr/local/bin/unilaunch \
		--desktop-file=../AppDir/UniLaunch.desktop \
		--output=appimage \
		-i ../AppDir/UniLaunch.png
