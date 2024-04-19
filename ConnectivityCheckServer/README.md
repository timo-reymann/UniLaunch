Connectivity Checker
===

Connectivity checker that is used by UniLaunch to verify internet connection works.

# Disclaimer

This image is primarily intended to be used by UniLaunch to check for connectivity. This is set as default and basically
works with any HTTP url that returns a 2xx or 3xx response.

That being said, I will host this on my own server under `https://cpnnectivity-check.timo-reymann.de`. You are free to
use this for UniLaunch or any other software.

If you dont trust that or want to use another one you can either configure it in UniLaunch or set up this container on
your vServer, VPS, VM, Raspberry PI or whatever.

# Set up

The example showcases set up using docker-compose feel free to use what ever you like to spin up the container:

```yaml
version: '3.2'
services:
  connectivity-checker:
    image: timoreymann/connectivity-checker
    ports:
      - "80:8080"
    environment:
      UC_PRIVACY_URL: https://privacy.example.com
```
