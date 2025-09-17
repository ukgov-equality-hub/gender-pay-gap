#!/bin/bash

set -x

podman run \
  --name gpg-postgres--container \
  --env 'POSTGRES_DB=gpg' \
  --env 'POSTGRES_USER=postgres' \
  --env 'POSTGRES_PASSWORD=password' \
  --publish 5432:5432 \
  --volume pgdata:/var/lib/postgresql/data \
  --replace \
  postgres:16
