#!/bin/sh

./startup.sh -tf ./config.tenants/tenants.txt
sleep 10
./startup.sh -b ./config
