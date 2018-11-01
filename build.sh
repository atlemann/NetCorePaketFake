#!/usr/bin/env bash

set -eu

dotnet restore build.proj
dotnet fake build