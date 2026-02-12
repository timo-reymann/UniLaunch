FROM golang:1.26 as build
WORKDIR /build
COPY main.go ./
RUN CGO_ENABLED=0 go build -ldflags "-s -w" -o connectivity-check main.go

FROM scratch
LABEL org.opencontainers.image.title="connecitvity-check"
LABEL org.opencontainers.image.description="Connectivity checker that is used by UniLaunch to verify internet connection works."
LABEL org.opencontainers.image.ref.name="main"
LABEL org.opencontainers.image.licenses='GNU GPL v3'
LABEL org.opencontainers.image.vendor="Timo Reymann <mail@timo-reymann.de>"
LABEL org.opencontainers.image.authors="Timo Reymann <mail@timo-reymann.de>"
LABEL org.opencontainers.image.url="https://github.com/timo-reymann/UniLaunch"
LABEL org.opencontainers.image.documentation="https://github.com/timo-reymann/UniLaunch"
LABEL org.opencontainers.image.source="https://github.com/timo-reymann/UniLaunch.git"
COPY --from=gcr.io/distroless/static-debian12:nonroot / /
USER nonroot
COPY --from=build /build/connectivity-check /bin/connectivity-check
ENTRYPOINT ["/bin/connectivity-check"]
