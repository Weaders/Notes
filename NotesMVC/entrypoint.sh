#!/bin/bash

set -e
run_cmd="dotnet NotesMVC.dll --server.urls http://*:80"

cd /src/NotesMVC

until dotnet ef database update; do
echo "Sql serer is starting";
sleep 1
done

cd /app

echo "SQL server is up";
exec $run_cmd