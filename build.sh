#!/usr/bin/env bash

docker build DaprPluggableComponent/ -t  ghcr.io/fjvela/dapr/pluggable-component:latest
docker push ghcr.io/fjvela/dapr/pluggable-component:latest

docker build MyWebApi/ -t  ghcr.io/fjvela/dapr/my-webapi:latest
docker push ghcr.io/fjvela/dapr/my-webapi:latest
