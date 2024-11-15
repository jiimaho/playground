#!/bin/bash

# Wait for SQL Server to be available
echo "Waiting for SQL Server to be available..."

# Loop until the SQL Server is available
for i in {1..50};
do
  /opt/mssql-tools18/bin/sqlcmd -S sql -U SA -P reallyStrongPwd123 -C -Q "SELECT 1" > /dev/null 2>&1
  if [ $? -eq 0 ]
  then
    echo "SQL Server is ready"
    break
  else
    echo "SQL Server not ready yet, sleeping"
    sleep 1
  fi 
done

/opt/mssql-tools18/bin/sqlcmd -S sql -U SA -P reallyStrongPwd123 -C -d master -i /init.sql