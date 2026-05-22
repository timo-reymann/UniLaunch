FROM golang:1.26 AS build
ENV CGO_ENABLED=0
WORKDIR /build
COPY main.go ./
RUN go build -ldflags "-s -w" -o connectivity-check main.go

FROM timoreymann/ubuntu-runtime:26.8
LABEL org.opencontainers.image.title="connecitvity-check" \
      org.opencontainers.image.description="Connectivity checker that is used by UniLaunch to verify internet connection works." \
      org.opencontainers.image.ref.name="main" \
      org.opencontainers.image.licenses='GNU GPL v3' \
      org.opencontainers.image.vendor="Timo Reymann <mail@timo-reymann.de>" \
      org.opencontainers.image.authors="Timo Reymann <mail@timo-reymann.de>" \
      org.opencontainers.image.url="https://github.com/timo-reymann/UniLaunch" \
      org.opencontainers.image.documentation="https://github.com/timo-reymann/UniLaunch" \
      org.opencontainers.image.source="https://github.com/timo-reymann/UniLaunch.git"

COPY --from=build /build/connectivity-check /bin/connectivity-check

ENTRYPOINT ["/bin/connectivity-check"]
