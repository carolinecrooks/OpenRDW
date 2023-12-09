#!/bin/bash

REMOTE_USER="tahas"
REMOTE_HOST="tshaikpi1"
REMOTE_FILE="~/Documents/final/locrot_data.txt"
LOCAL_IP="172.16.0.3"
LOCAL_DESTINATION="$PWD"
echo "Hi"
while true; do
	sshpass -p "tahas" scp "$REMOTE_USER@$REMOTE_HOST:$REMOTE_FILE" "$LOCAL_DESTINATION"
	echo "Transferred."
	cat locrot_data.txt
	sleep 0.07
done

