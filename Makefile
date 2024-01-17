SHELL := /bin/bash
.PHONY: help

help: ## Display this help page
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[33m%-30s\033[0m %s\n", $$1, $$2}'

build: build-macos-app ## Build all supported platforms

build-macos-app: ## Build the MacOS app
	cd UniLaunch.MacOS/ && \
	dotnet publish UniLaunch.MacOS.csproj -c Release && \
	rm -rf ../dist || true && \
	mkdir -p ../dist/UniLaunch.app/Contents/MacOS && \
	cp -r Resources/* ../dist/UniLaunch.app/Contents  && \
	mkdir -p ../dist/UniLaunch.app/Contents && \
	cp bin/Release/net7.0/osx-arm64/publish/UniLaunch.MacOS ../dist/UniLaunch.app/Contents/MacOS/UniLaunch

deploy-local-macos: ## Deploy MacOS locally
	sudo cp -r dist/UniLaunch.app /Applications/
